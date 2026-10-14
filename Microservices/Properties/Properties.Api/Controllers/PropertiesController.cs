using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Properties.Api.DTOs.Properties;
using Properties.Application.UseCases.Properties.Commands.CreateProperty;
using Properties.Application.UseCases.Properties.Commands.DeleteProperty;
using Properties.Application.UseCases.Properties.Commands.UpdateProperty;
using Properties.Application.UseCases.Properties.Queries.GetPropertiesList;
using Properties.Application.UseCases.Properties.Queries.GetPropertyById;
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
                return ToFailure(result);
            }

            //return Ok(result);
            return StatusCode(StatusCodes.Status200OK, result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Result<PropertyDetailsDTO> result = await _mediator.Send(new GetPropertyByIdQuery { Id = id });
            return result.IsSuccess ? Ok(result.Value) : ToFailure(result);
        }

        [HttpPost]
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
                return ToFailure(result);
            }

            return StatusCode(StatusCodes.Status201Created, new { id = result.Value });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePropertyDTO request)
        {
            UpdatePropertyCommand command = new()
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                PriceAmount = request.PriceAmount,
                CurrencyCode = request.CurrencyCode,
                PropertyTypeId = request.PropertyTypeId,
                NeighborhoodId = request.NeighborhoodId,
                Address = ToAddressInput(request.Address),
                Bedrooms = request.Bedrooms,
                ParkingSpaces = request.ParkingSpaces,
                Bathrooms = request.Bathrooms,
                Stratum = request.Stratum,
                Area = request.Area,
                Condition = request.Condition,
                IsAvailable = request.IsAvailable!.Value,
                Amenities = request.Amenities
            };

            Result<Guid> result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : ToFailure(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Result<Guid> result = await _mediator.Send(new DeletePropertyCommand { Id = id });
            return result.IsSuccess ? NoContent() : ToFailure(result);
        }

        private static PropertyAddressInput ToAddressInput(PropertyAddressRequest address) => new()
        {
            MainRoadType = address.MainRoadType,
            MainRoadNumber = address.MainRoadNumber,
            MainRoadLetter = address.MainRoadLetter,
            MainRoadSuffix = address.MainRoadSuffix,
            CrossRoadNumber = address.CrossRoadNumber,
            CrossRoadLetter = address.CrossRoadLetter,
            CrossRoadSuffix = address.CrossRoadSuffix,
            Plate = address.Plate,
            Indications = address.Indications,
            PostalCode = address.PostalCode,
            Latitude = address.Latitude,
            Longitude = address.Longitude
        };

        private IActionResult ToFailure(Result result)
        {
            if (result.Errors.Any(error => error.Type == ErrorType.NotFound))
            {
                return NotFound(result.Errors);
            }

            if (result.Errors.All(error => error.Type is ErrorType.Validation or ErrorType.Business))
            {
                return BadRequest(result.Errors);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
        }
    }
}
