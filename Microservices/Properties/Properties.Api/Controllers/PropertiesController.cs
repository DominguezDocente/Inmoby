using Microsoft.AspNetCore.Mvc;
using Properties.Application.UseCases.Properties.Queries.GetPropertiesList;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;

namespace Properties.Api.Controllers
{
    [ApiController]
    [Route("api/properties")]
    public class PropertiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
            [FromQuery] Guid? propertyTypeId = null,
            [FromQuery] Guid? cityId = null,
            [FromQuery] int? stratum = null)
        {
            GetPropertiesListQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize),
                PropertyTypeId = propertyTypeId,
                CityId = cityId,
                Stratum = stratum
            };

            PaginationResponse<PropertyListItemDTO> result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
