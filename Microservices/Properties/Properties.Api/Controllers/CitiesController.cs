using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Properties.Api.DTOs.Locations;
using Properties.Application.UseCases.Locations.Commands.CreateCity;
using Properties.Application.UseCases.Locations.Commands.DeleteCity;
using Properties.Application.UseCases.Locations.Commands.UpdateCity;
using Properties.Application.UseCases.Locations.Queries.GetCitiesList;
using Properties.Application.UseCases.Locations.Queries.GetCityById;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Application.Utilities.Results;

namespace Properties.Api.Controllers
{
    [Route("api/locations/cities")]
    [Authorize]
    public class CitiesController : ApiController
    {
        private readonly IMediator _mediator;

        public CitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1,
                                                 [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                                 [FromQuery] Guid? stateId = null,
                                                 [FromQuery] string? filter = null)
        {
            GetCitiesListQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize),
                StateId = stateId,
                Filter = filter
            };

            Result<PaginationResponse<CityDTO>> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Result<CityDTO> result = await _mediator.Send(new GetCityByIdQuery { Id = id });

            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCityDTO request)
        {
            CreateCityCommand command = new()
            {
                Name = request.Name,
                StateId = request.StateId
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result, StatusCodes.Status201Created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCityDTO request)
        {
            UpdateCityCommand command = new()
            {
                Id = id,
                Name = request.Name,
                StateId = request.StateId
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<Guid> result = await _mediator.Send(new DeleteCityCommand { Id = id });

            return ToActionResult(result);
        }
    }
}
