using Properties.Application.Contracts.Repositories;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Properties;
using Properties.Application.Utilities.Results;
using Properties.Application.UseCases.Common;

namespace Properties.Application.UseCases.Properties.Queries.GetPropertiesList
{
    public class GetPropertiesListUseCase : 
        BaseHandler,
        IRequestHandler<GetPropertiesListQuery, Result<PaginationResponse<PropertyListItemDTO>>>
    {
        private readonly IPropertiesRepository _repository;

        public GetPropertiesListUseCase(IPropertiesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PaginationResponse<PropertyListItemDTO>>> Handle(GetPropertiesListQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                PaginationRequest pagination = query.Pagination;

                PaginationResponse<Property> response = await _repository.GetPagedListAsync(pagination,
                                                                                            query.PropertyTypeId,
                                                                                            query.CityId,
                                                                                            query.Stratum);

                List<PropertyListItemDTO> items = response.Items.Select(p => p.ToListItemDTO())
                                                                .ToList();

                return PaginationResponse<PropertyListItemDTO>.Create(items, response.TotalCount, pagination);
            });
        }
    }
}
