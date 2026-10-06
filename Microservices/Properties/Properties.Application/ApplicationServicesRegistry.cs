using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Properties.Application.UseCases.Properties.Commands.CreateProperty;
using Properties.Application.UseCases.Properties.Queries.GetPropertiesList;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Application.Utilities.Results;
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
            services.AddScoped<IRequestHandler<GetPropertiesListQuery, Result<PaginationResponse<PropertyListItemDTO>>>, GetPropertiesListUseCase>();

            // Validations
            services.AddValidatorsFromAssemblyContaining<CreatePropertyCommandValidator>();

            return services;
        }
    }
}
