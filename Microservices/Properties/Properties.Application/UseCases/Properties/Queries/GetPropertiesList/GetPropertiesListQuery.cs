using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.UseCases.Properties.Queries.GetPropertiesList
{
    public class GetPropertiesListQuery : IRequest<Result<PaginationResponse<PropertyListItemDTO>>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();

        public Guid? PropertyTypeId { get; set; }
        public Guid? CityId { get; set; }
        public int? Stratum { get; set; }
    }
}
