using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Properties.Api.DTOs.PropertyTypes;
using Properties.Application.UseCases.PropertyTypes.Commands.CreatePropertyType;
using Properties.Application.UseCases.PropertyTypes.Commands.DeletePropertyType;
using Properties.Application.UseCases.PropertyTypes.Commands.UpdatePropertyType;
using Properties.Application.UseCases.PropertyTypes.Queries.GetPropertyTypeById;
using Properties.Application.UseCases.PropertyTypes.Queries.GetPropertyTypesList;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Application.Utilities.Results;

namespace Properties.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PropertyTypesController : ApiController
    {
        private readonly IMediator _mediator;

        public PropertyTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1,
                                                 [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                                 [FromQuery] string? filter = null)
        {
            GetPropertyTypesListQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize),
                Filter = filter
            };

            Result<PaginationResponse<PropertyTypeDTO>> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Result<PropertyTypeDTO> result = await _mediator.Send(new GetPropertyTypeByIdQuery { Id = id });

            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePropertyTypeDTO request)
        {
            CreatePropertyTypeCommand command = new()
            {
                Name = request.Name,
                Description = request.Description
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result, StatusCodes.Status201Created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePropertyTypeDTO request)
        {
            UpdatePropertyTypeCommand command = new()
            {
                Id = id,
                Name = request.Name,
                Description = request.Description
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<Guid> result = await _mediator.Send(new DeletePropertyTypeCommand { Id = id });

            return ToActionResult(result);
        }
    }
}
