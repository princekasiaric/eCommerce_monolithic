using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Ardalis.GuardClauses;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.Models.OrderAggregate;
using Construmart.Core.Domain.SeedWork;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using Construmart.Core.ProcessorContracts.Paystack;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using static Construmart.Core.ProcessorContracts.Paystack.DTOs;

namespace Construmart.Core.UseCases.OrderUseCases
{
    public class CreateOrderCommand : RequestContext<BaseResponse>
    {
        public long CartId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public long DeliveryAddressId { get; private set; }
        public string PaymentRefrence { get; private set; }
        public ClaimsPrincipal ClaimsPrincipal { get; private set; }

        public CreateOrderCommand(OrderRequest request, ClaimsPrincipal claimsPrincipal)
        {
            CartId = request.CartId;
            FirstName = HttpUtility.HtmlEncode(request.FirstName.Trim().ToLower());
            LastName = HttpUtility.HtmlEncode(request.LastName.Trim().ToLower()); ;
            PhoneNumber = request.PhoneNumber;
            DeliveryAddressId = request.DeliveryAddressId;
            PaymentRefrence = request.PaymentRefrence;
            ClaimsPrincipal = claimsPrincipal;
        }
    }

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(11)
                .Matches(new Regex(Constants.AppRegex.PHONE_NUMBER, RegexOptions.Compiled))
                .WithMessage("Phone number must be 11 digits.");
            RuleFor(x => x.DeliveryAddressId).GreaterThan(0);
            RuleFor(x => x.PaymentRefrence).NotEmpty();
        }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, BaseResponse>, IDisposable
    {
        private readonly IIdentityService _identityService;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IVerifyTransactionService _verifyTransaction;
        private readonly IMapper _mapper;
        private readonly IResult _result;

        public CreateOrderCommandHandler(
                IIdentityService identityService,
                IRepositoryManager repositoryManager,
                IVerifyTransactionService verifyTransaction,
                IMapper mapper,
                IResult result)
        {
            _identityService = Guard.Against.Null(identityService, nameof(identityService));
            _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
            _verifyTransaction = Guard.Against.Null(verifyTransaction, nameof(verifyTransaction));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _result = Guard.Against.Null(result, nameof(result));
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
        }

        public async Task<BaseResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var identityResult = _identityService.GetUserIdFromClaims(request.ClaimsPrincipal);

            if (!identityResult.IsSuccess)
            {
                return identityResult;
            }

            var userIdResult = identityResult as ServiceResponse<UserIdResponse>;

            var customer = await _repositoryManager.CustomerRepo
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userIdResult.Payload.ApplicationUserId);

            if (customer == null)
            {
                return _result.Failure(ResponseCodes.InvalidUserAccount, StatusCodes.Status404NotFound);
            }

            var cart = await _repositoryManager.CartRepo.SingleOrDefaultAsync(
                x => x.Id == request.CartId,
                includes: new System.Linq.Expressions.Expression<Func<Domain.Models.Cart, object>>[]
                {
                    x => x.CartItems
                },
                withTracking: true
            );

            if (cart == null)
            {
                return _result.Failure(ResponseCodes.InvalidCart, StatusCodes.Status404NotFound);
            }

            if (!cart.CartItems.Any())
            {
                return _result.Failure(ResponseCodes.EmptyCart);
            }

            await _repositoryManager.BeginTransactionAsync();

            var deliveryAddress = await _repositoryManager.DeliveryAddressRepo.SingleOrDefaultAsync(x => x.Id == request.DeliveryAddressId);

            if (deliveryAddress == null)
            {
                return _result.Failure(ResponseCodes.InvalidDeliveryAddress, StatusCodes.Status404NotFound);
            }

            var nigerianState = await _repositoryManager.NigerianStateRepo.SingleOrDefaultAsync(x => x.Id == deliveryAddress.NigerianStateId);

            if (nigerianState == null)
            {
                return _result.Failure(ResponseCodes.InvalidNigerianState, StatusCodes.Status404NotFound);
            }

            //verify payment
            var (isSuccess, jsonResponse) = await _verifyTransaction.VerifyTransaction(request.PaymentRefrence);
            if (!isSuccess)
                return _result.Failure(ResponseCodes.TransactionVerification);

            var transResponse = JsonSerializer.Deserialize<TransactionVerificationResponse>(jsonResponse);
            if (!transResponse.Status)
                return _result.Failure(ResponseCodes.InvalidTransaction);

            var order = Order.Create(
                customer.Id,
                Guid.NewGuid().ToString("N"),
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                deliveryAddress.Address,
                deliveryAddress.City,
                deliveryAddress.LGA,
                state: nigerianState.State,
                EnumerationBase.FromDisplayName<OrderStatus>(OrderStatus.Initiated.DisplayName));

            foreach (var cartItem in cart.CartItems)
            {
                var product = await _repositoryManager.ProductRepo.SingleOrDefaultAsync(x => x.Id == cartItem.ProductId);
                if (product != null)
                {
                    var discount = await _repositoryManager.DiscountRepo.SingleOrDefaultAsync(x => x.Id == product.DiscountId);
                    var percentageOff = (discount != null) ? (discount.PercentageOff * 0.01) : 0;
                    order.AddOrderItem(product.Id, product.Name, product.UnitPrice, cartItem.Quantity, percentageOff);
                }
                else
                {
                    return _result.Failure(ResponseCodes.InvalidProduct, StatusCodes.Status404NotFound);
                }
            }

            var totalOrderPrice = order.OrderItems.Sum(x =>
            {
                var discountedPrice = x.UnitPrice - (x.UnitPrice * Convert.ToDecimal(x.Discount));
                var totalPrice = discountedPrice * x.Quantity;
                return totalPrice;
            });

            order.SetOrderTotalAmount(totalOrderPrice);

            await _repositoryManager.OrderRepo.AddAsync(order);
            await _repositoryManager.SaveAsync();

            if (order.Id <= 0)
                return _result.Failure(ResponseCodes.GeneralError, StatusCodes.Status500InternalServerError);

            cart.Checkout();

            await _repositoryManager.SaveAsync();

            var transaction = Transaction.Create(
                order.TrackingNumber,
                request.PaymentRefrence,
                EnumerationBase.FromDisplayName<TransactionStatus>(TransactionStatus.Success.DisplayName),
                jsonResponse);

            await _repositoryManager.TransactionRepo.AddAsync(transaction);
            await _repositoryManager.SaveAsync();

            await _repositoryManager.CommitAsync();

            var response = _mapper.Map<OrderResponse>(order);
            return _result.Success(response);
        }
    }
}
