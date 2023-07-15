using System.Net.Mime;
using System.Threading.Tasks;
using Construmart.Api.Filters;
using Construmart.Core.Commons;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.UseCases.AuthUseCases;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Construmart.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route(Routes.ROOT)]
    public class AuthController : RootController
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="xClient"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServiceResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerHeader(
            Constants.CustomHeaderNames.XClient,
            "The name of the registered application performing this operation",
            isRequired: true)]
        [HttpPost(Routes.LOGIN, Name = nameof(Routes.LOGIN))]
        public async Task<IActionResult> LoginAsync(
            [FromHeader(Name = Constants.CustomHeaderNames.XClient)] string xClient,
            [FromBody] LoginRequest request)
            => ResolveActionResult(await _mediator.Send(new LoginCommand(request, xClient)));

        /// Changes password for authenticated user
        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerHeader(
            Constants.CustomHeaderNames.XClient,
            "The name of the registered application performing this operation",
            isRequired: true)]
        [HttpPut(Routes.CHANGE_PASSWORD, Name = nameof(Routes.CHANGE_PASSWORD))]
        public async Task<IActionResult> ChangePasswordAsync(
            [FromHeader(Name = Constants.CustomHeaderNames.XClient)] string xClient,
            [FromBody] ChangePasswordRequest request)
            => ResolveActionResult(await _mediator.Send(new ChangePasswordCommand(request, xClient)));

        /// <summary>
        /// Initiates Password Reset
        /// </summary>
        /// <param name="xClient"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerHeader(
            Constants.CustomHeaderNames.XClient,
            "The name of the registered application performing this operation",
            isRequired: true)]
        [HttpPost(Routes.INITIATE_RESET_PASSWORD, Name = nameof(Routes.INITIATE_RESET_PASSWORD))]
        public async Task<IActionResult> InitiateResetPassword(
            [FromHeader(Name = Constants.CustomHeaderNames.XClient)] string xClient,
            [FromBody] InitiateResetPaswordRequest request)
            => ResolveActionResult(await _mediator.Send(new InitiateResetPasswordCommand(request, xClient)));

        /// <summary>
        /// Completes Password Reset
        /// </summary>
        /// <param name="xClient"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerHeader(
            Constants.CustomHeaderNames.XClient,
            "The name of the registered application performing this operation",
            isRequired: true)]
        [HttpPut(Routes.COMPLETE_RESET_PASSWORD, Name = nameof(Routes.COMPLETE_RESET_PASSWORD))]
        public async Task<IActionResult> CompletePasswordReset(
            [FromHeader(Name = Constants.CustomHeaderNames.XClient)] string xClient,
            [FromBody] CompleteResetPasswordRequest request)
            => ResolveActionResult(await _mediator.Send(new CompleteResetPasswordCommand(request, xClient)));
    }
}