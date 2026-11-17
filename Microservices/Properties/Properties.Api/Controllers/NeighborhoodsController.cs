using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Properties.Api.DTOs.Locations;
using Properties.Application.UseCases.Locations.Commands.CreateNeighborhood;
using Properties.Application.UseCases.Locations.Commands.DeleteNeighborhood;
using Properties.Application.UseCases.Locations.Commands.UpdateNeighborhood;
using Properties.Application.UseCases.Locations.Queries.GetNeighborhoodById;
using Properties.Application.UseCases.Locations.Queries.GetNeighborhoodsList;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Application.Utilities.Results;

namespace Properties.Api.Controllers
{
    [Route("api/locations/neighborhoods")]
    [Authorize]
    public class NeighborhoodsController : ApiController
    {
        private readonly IMediator _mediator;

        public NeighborhoodsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1,
                                                 [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                                 [FromQuery] Guid? cityId = null,
                                                 [FromQuery] string? filter = null)
        {
            GetNeighborhoodsListQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize),
                CityId = cityId,
                Filter = filter
            };

            Result<PaginationResponse<NeighborhoodDTO>> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Result<NeighborhoodDTO> result = await _mediator.Send(new GetNeighborhoodByIdQuery { Id = id });

            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNeighborhoodDTO request)
        {
            CreateNeighborhoodCommand command = new()
            {
                Name = request.Name,
                CityId = request.CityId
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result, StatusCodes.Status201Created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateNeighborhoodDTO request)
        {
            UpdateNeighborhoodCommand command = new()
            {
                Id = id,
                Name = request.Name,
                CityId = request.CityId
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<Guid> result = await _mediator.Send(new DeleteNeighborhoodCommand { Id = id });

            return ToActionResult(result);
        }
    }
}
