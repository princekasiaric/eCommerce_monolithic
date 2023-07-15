using System.Collections.Generic;
using System.Threading.Tasks;
using Construmart.Core.DTOs.Response;
using Construmart.Core.UseCases.NigerianStateUseCase;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Construmart.Api.Controllers
{
    [ApiController]
    [Route(Routes.ROOT)]
    public class NigerianStatesController : RootController
    {
        private readonly IMediator _mediator;

        public NigerianStatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ServiceResponse<NigerianStateResponse>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_NIGERIAN_STATE)]
        public async Task<IActionResult> ViewNigerianStateAsync(int id)
            => ResolveActionResult(await _mediator.Send(new ViewNigerianStateQuery(id)));

        [ProducesResponseType(typeof(ServiceResponse<IList<NigerianStateResponse>>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_NIGERIAN_STATES)]
        public async Task<IActionResult> ViewNigerianStatesAsync()
            => ResolveActionResult(await _mediator.Send(new ViewNigerianStatesQuery()));
    }
}
