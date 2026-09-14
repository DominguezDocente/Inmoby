using Microsoft.Extensions.DependencyInjection;
using Properties.Application.UseCases.Properties.Queries.GetPropertiesList;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Mediator
            services.AddScoped<IMediator, SimpleMediator>();

            // Use Cases
            services.AddScoped<IRequestHandler<GetPropertiesListQuery, PaginationResponse<PropertyListItemDTO>>, GetPropertiesListUseCase>();

            return services;
        }
    }
}
