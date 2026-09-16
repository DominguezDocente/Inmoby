using Properties.Application.Contracts.Repositories;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.UseCases.Properties.Queries.GetPropertiesList
{
    public class GetPropertiesListUseCase : IRequestHandler<GetPropertiesListQuery, PaginationResponse<PropertyListItemDTO>>
    {
        private readonly IPropertiesRepository _repository;

        public GetPropertiesListUseCase(IPropertiesRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginationResponse<PropertyListItemDTO>> Handle(GetPropertiesListQuery query)
        {
            PaginationRequest pagination = query.Pagination;

            PaginationResponse<Property> response = await _repository.GetPagedListAsync(pagination,
                                                                                        query.PropertyTypeId,
                                                                                        query.CityId,
                                                                                        query.Stratum);

            List<PropertyListItemDTO> items = response.Items.Select(p => p.ToListItemDTO())
                                                            .ToList();

            return PaginationResponse<PropertyListItemDTO>.Create(items, response.TotalCount, pagination);
        }
    }
}
