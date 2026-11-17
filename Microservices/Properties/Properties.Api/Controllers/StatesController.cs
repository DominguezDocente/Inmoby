using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Properties.Api.DTOs.Locations;
using Properties.Application.UseCases.Locations.Commands.CreateState;
using Properties.Application.UseCases.Locations.Commands.DeleteState;
using Properties.Application.UseCases.Locations.Commands.UpdateState;
using Properties.Application.UseCases.Locations.Queries.GetStateById;
using Properties.Application.UseCases.Locations.Queries.GetStatesList;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Application.Utilities.Results;

namespace Properties.Api.Controllers
{
    [Route("api/locations/states")]
    [Authorize]
    public class StatesController : ApiController
    {
        private readonly IMediator _mediator;

        public StatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1,
                                                 [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                                 [FromQuery] Guid? countryId = null,
                                                 [FromQuery] string? filter = null)
        {
            GetStatesListQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize),
                CountryId = countryId,
                Filter = filter
            };

            Result<PaginationResponse<StateDTO>> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Result<StateDTO> result = await _mediator.Send(new GetStateByIdQuery { Id = id });

            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStateDTO request)
        {
            CreateStateCommand command = new()
            {
                Name = request.Name,
                CountryId = request.CountryId
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result, StatusCodes.Status201Created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStateDTO request)
        {
            UpdateStateCommand command = new()
            {
                Id = id,
                Name = request.Name,
                CountryId = request.CountryId
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<Guid> result = await _mediator.Send(new DeleteStateCommand { Id = id });

            return ToActionResult(result);
        }
    }
}
