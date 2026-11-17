using Properties.Application.Contracts.Repositories;
using Properties.Application.UseCases.Common;
using Properties.Application.UseCases.PropertyTypes.Queries.GetPropertyTypeById;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Properties;

namespace Properties.Application.UseCases.PropertyTypes.Queries.GetPropertyTypesList
{
    public sealed class GetPropertyTypesListQuery : IRequest<Result<PaginationResponse<PropertyTypeDTO>>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();
        public string? Filter { get; set; }
    }

    public sealed class GetPropertyTypesListUseCase : BaseHandler, IRequestHandler<GetPropertyTypesListQuery, Result<PaginationResponse<PropertyTypeDTO>>>
    {
        private readonly IPropertyTypesRepository _repository;

        public GetPropertyTypesListUseCase(IPropertyTypesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PaginationResponse<PropertyTypeDTO>>> Handle(GetPropertyTypesListQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                PaginationResponse<PropertyType> response = await _repository.GetPagedListAsync(query.Pagination,
                                                                                                 query.Filter);

                List<PropertyTypeDTO> items = response.Items.Select(propertyType => new PropertyTypeDTO
                {
                    Id = propertyType.Id,
                    Name = propertyType.Name,
                    Description = propertyType.Description
                }).ToList();

                return PaginationResponse<PropertyTypeDTO>.Create(items, response.TotalCount, query.Pagination);
            });
        }
    }
}
