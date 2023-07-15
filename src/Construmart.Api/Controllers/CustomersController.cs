using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.UseCases.CustomerUseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Construmart.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route(Routes.ROOT)]
    public class CustomersController : RootController
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public CustomersController(IMediator mediator)
        {
            _mediator = Guard.Against.Null(mediator, nameof(mediator));
        }

        /// <summary>
        /// Initiates Customer Onboarding
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost(Routes.INITIATE_CUSTOMER_SIGNUP, Name = nameof(Routes.INITIATE_CUSTOMER_SIGNUP))]
        public async Task<IActionResult> InitiateCustomerSignupAsync([FromBody] InitiateCustomerSignupRequest request)
        {
            // var endpointName = HttpContext.GetEndpoint().Metadata.GetMetadata<EndpointNameMetadata>().EndpointName;
            return ResolveActionResult(await _mediator.Send(new InitiateCustomerSignupCommand(request)));
        }

        /// <summary>
        /// Completes Customer Onboarding
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServiceResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPut(Routes.COMPLETE_CUSTOMER_SIGNUP, Name = nameof(Routes.COMPLETE_CUSTOMER_SIGNUP))]
        public async Task<IActionResult> CompleteCustomerSignupAsync([FromBody] CompleteCustomerSignupRequest request)
            => ResolveActionResult(await _mediator.Send(new CompleteCustomerSignupCommand(request)));

        /// <summary>
        /// View Customer Account Details
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServiceResponse<CustomerAccountInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(RoleTypes.Customer))]
        [HttpGet(Routes.CUSTOMER_VIEW_ACCOUNT_DETAILS, Name = nameof(Routes.CUSTOMER_VIEW_ACCOUNT_DETAILS))]
        public async Task<IActionResult> ViewAccountDetails()
            => ResolveActionResult(await _mediator.Send(new CustomerAccountInfoQuery(User)));

        /// <summary>
        /// Update customer's details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(RoleTypes.Customer))]
        [ProducesResponseType(typeof(ServiceResponse<LoginResponse>), StatusCodes.Status200OK)]
        [HttpPut(Routes.CUSTOMER_UPDATE_ACCOUNT_DETAILS, Name = nameof(Routes.CUSTOMER_UPDATE_ACCOUNT_DETAILS))]
        public async Task<IActionResult> UpdateCustomerDetails([FromBody] UpdateCustomerDetailsRequest request)
        => ResolveActionResult(await _mediator.Send(new UpdateCustomerDetailsCommand(request)));
    }
}