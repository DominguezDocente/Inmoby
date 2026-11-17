using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Properties;

namespace Properties.Application.UseCases.PropertyTypes.Queries.GetPropertyTypeById
{
    public sealed class GetPropertyTypeByIdQuery : IRequest<Result<PropertyTypeDTO>>
    {
        public Guid Id { get; set; }
    }

    public sealed class PropertyTypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public sealed class GetPropertyTypeByIdUseCase : BaseHandler, IRequestHandler<GetPropertyTypeByIdQuery, Result<PropertyTypeDTO>>
    {
        private readonly IPropertyTypesRepository _repository;

        public GetPropertyTypeByIdUseCase(IPropertyTypesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PropertyTypeDTO>> Handle(GetPropertyTypeByIdQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                PropertyType? propertyType = await _repository.GetByIdAsync(query.Id);

                if (propertyType is null)
                {
                    throw new NotFoundException($"No existe un tipo de inmueble con ID {query.Id}.");
                }

                return new PropertyTypeDTO
                {
                    Id = propertyType.Id,
                    Name = propertyType.Name,
                    Description = propertyType.Description
                };
            });
        }
    }
}
