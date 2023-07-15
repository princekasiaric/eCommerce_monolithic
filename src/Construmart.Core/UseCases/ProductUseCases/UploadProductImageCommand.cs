using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.FileStorage;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.ProductUseCases
{
    public class UploadProductImageCommand : RequestContext<BaseResponse>
    {
        public string Base64String { get; private set; }
        public long ProductId { get; private set; }
        public ClaimsPrincipal ClaimsPrincipal { get; }

        public UploadProductImageCommand(long productId, ImageUploadRequest request, ClaimsPrincipal claimsPrincipal)
        {
            Base64String = request.Base64String;
            ProductId = productId;
            ClaimsPrincipal = claimsPrincipal;
        }
    }

    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IFileStorageService _fileStorageService;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IIdentityService _identityService;

        public UploadProductImageCommandHandler(
            IResult result,
            IFileStorageService fileStorageService,
            IRepositoryManager repositoryManager,
            IIdentityService identityService)
        {
            _result = Guard.Against.Null(result, nameof(result));
            _fileStorageService = Guard.Against.Null(fileStorageService, nameof(fileStorageService));
            _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
            _identityService = Guard.Against.Null(identityService, nameof(identityService));
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<BaseResponse> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
        {
            var product = await _repositoryManager.ProductRepo.SingleOrDefaultAsync(
                x => x.Id == request.ProductId,
                includes: new Expression<Func<Domain.Models.ProductAggregate.Product, object>>[] { x => x.ProductImage });
            if (product == null)
            {
                return _result.Failure(ResponseCodes.RecordNotFound, StatusCodes.Status404NotFound);
            }
            var identityResult = _identityService.GetUserIdFromClaims(request.ClaimsPrincipal);
            var userIdResult = identityResult as ServiceResponse<UserIdResponse>;
            var (isSuccessful, msg, data) = await _fileStorageService.UploadFileAsync(request.Base64String, Domain.Enumerations.FileTypes.Image, "product");
            if (!isSuccessful || data == null)
            {
                return _result.Failure(ResponseCodes.FileUploadFailure, StatusCodes.Status400BadRequest, msg);
            }
            product.UploadImage(data.Url, data.SecureUrl);
            _repositoryManager.ProductRepo.Update(product);
            await _repositoryManager.SaveAsync();
            return _result.Success(
                new UploadProductImageResponse(
                    product.Id,
                    data.Url,
                    data.SecureUrl),
                StatusCodes.Status201Created);
        }
    }
}