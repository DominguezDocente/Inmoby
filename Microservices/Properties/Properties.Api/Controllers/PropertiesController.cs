using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Properties.Api.DTOs.Properties;
using Properties.Application.UseCases.Properties.Commands.CreateProperty;
using Properties.Application.UseCases.Properties.Queries.GetPropertiesList;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Application.Utilities.Results;

namespace Properties.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1,
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

            Result<PaginationResponse<PropertyListItemDTO>> result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            //return Ok(result);
            return StatusCode(StatusCodes.Status200OK, result.Value);
        }
        public async Task<IActionResult> Create([FromBody] CreatePropertyDTO request)
        {
            CreatePropertyCommand command = new()
            {
                OwnerId = request.OwnerId,
                Title = request.Title,
                Description = request.Description,
                PriceAmount = request.PriceAmount,
                CurrencyCode = request.CurrencyCode,
                PropertyTypeId = request.PropertyTypeId,
                NeighborhoodId = request.NeighborhoodId,
                Address = new PropertyAddressInput
                {
                    MainRoadType = request.Address.MainRoadType,
                    MainRoadNumber = request.Address.MainRoadNumber,
                    MainRoadLetter = request.Address.MainRoadLetter,
                    MainRoadSuffix = request.Address.MainRoadSuffix,
                    CrossRoadNumber = request.Address.CrossRoadNumber,
                    CrossRoadLetter = request.Address.CrossRoadLetter,
                    CrossRoadSuffix = request.Address.CrossRoadSuffix,
                    Plate = request.Address.Plate,
                    Indications = request.Address.Indications,
                    PostalCode = request.Address.PostalCode,
                    Latitude = request.Address.Latitude,
                    Longitude = request.Address.Longitude
                },
                Bedrooms = request.Bedrooms,
                ParkingSpaces = request.ParkingSpaces,
                Bathrooms = request.Bathrooms,
                Stratum = request.Stratum,
                Area = request.Area,
                Condition = request.Condition,
                Amenities = request.Amenities
            };

            Result<Guid> result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return StatusCode(StatusCodes.Status201Created, new { id = result.Value });
        }
    }
}
