using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Properties.Api.DTOs.Locations;
using Properties.Application.UseCases.Locations.Commands.CreateCountry;
using Properties.Application.UseCases.Locations.Commands.DeleteCountry;
using Properties.Application.UseCases.Locations.Commands.UpdateCountry;
using Properties.Application.UseCases.Locations.Queries.GetCountriesList;
using Properties.Application.UseCases.Locations.Queries.GetCountryById;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Application.Utilities.Results;

namespace Properties.Api.Controllers
{
    [Route("api/locations/countries")]
    [Authorize]
    public class CountriesController : ApiController
    {
        private readonly IMediator _mediator;

        public CountriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1,
                                                 [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                                 [FromQuery] string? filter = null)
        {
            GetCountriesListQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize),
                Filter = filter
            };

            Result<PaginationResponse<CountryDTO>> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Result<CountryDTO> result = await _mediator.Send(new GetCountryByIdQuery { Id = id });

            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCountryDTO request)
        {
            CreateCountryCommand command = new()
            {
                Name = request.Name
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result, StatusCodes.Status201Created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCountryDTO request)
        {
            UpdateCountryCommand command = new()
            {
                Id = id,
                Name = request.Name
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<Guid> result = await _mediator.Send(new DeleteCountryCommand { Id = id });

            return ToActionResult(result);
        }
    }
}
