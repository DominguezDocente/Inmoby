using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Properties.Application.UseCases.Locations.Commands.CreateCity;
using Properties.Application.UseCases.Locations.Commands.CreateCountry;
using Properties.Application.UseCases.Locations.Commands.CreateNeighborhood;
using Properties.Application.UseCases.Locations.Commands.CreateState;
using Properties.Application.UseCases.Locations.Commands.DeleteCity;
using Properties.Application.UseCases.Locations.Commands.DeleteCountry;
using Properties.Application.UseCases.Locations.Commands.DeleteNeighborhood;
using Properties.Application.UseCases.Locations.Commands.DeleteState;
using Properties.Application.UseCases.Locations.Commands.UpdateCity;
using Properties.Application.UseCases.Locations.Commands.UpdateCountry;
using Properties.Application.UseCases.Locations.Commands.UpdateNeighborhood;
using Properties.Application.UseCases.Locations.Commands.UpdateState;
using Properties.Application.UseCases.Locations.Queries.GetCitiesList;
using Properties.Application.UseCases.Locations.Queries.GetCityById;
using Properties.Application.UseCases.Locations.Queries.GetCountriesList;
using Properties.Application.UseCases.Locations.Queries.GetCountryById;
using Properties.Application.UseCases.Locations.Queries.GetNeighborhoodById;
using Properties.Application.UseCases.Locations.Queries.GetNeighborhoodsList;
using Properties.Application.UseCases.Locations.Queries.GetStateById;
using Properties.Application.UseCases.Locations.Queries.GetStatesList;
using Properties.Application.UseCases.Properties.Commands.CreateProperty;
using Properties.Application.UseCases.Properties.Commands.DeleteProperty;
using Properties.Application.UseCases.Properties.Commands.UpdateProperty;
using Properties.Application.UseCases.Properties.Queries.GetPropertiesList;
using Properties.Application.UseCases.Properties.Queries.GetPropertyById;
using Properties.Application.UseCases.PropertyTypes.Commands.CreatePropertyType;
using Properties.Application.UseCases.PropertyTypes.Commands.DeletePropertyType;
using Properties.Application.UseCases.PropertyTypes.Commands.UpdatePropertyType;
using Properties.Application.UseCases.PropertyTypes.Queries.GetPropertyTypeById;
using Properties.Application.UseCases.PropertyTypes.Queries.GetPropertyTypesList;
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
            services.AddScoped<IRequestHandler<GetPropertyByIdQuery, Result<PropertyDetailsDTO>>, GetPropertyByIdUseCase>();
            services.AddScoped<IRequestHandler<CreatePropertyCommand, Result<Guid>>, CreatePropertyUseCase>();
            services.AddScoped<IRequestHandler<DeletePropertyCommand, Result<Guid>>, DeletePropertyUseCase>();
            services.AddScoped<IRequestHandler<UpdatePropertyCommand, Result<Guid>>, UpdatePropertyUseCase>();

            // Property Types
            services.AddScoped<IRequestHandler<GetPropertyTypesListQuery, Result<PaginationResponse<PropertyTypeDTO>>>, GetPropertyTypesListUseCase>();
            services.AddScoped<IRequestHandler<GetPropertyTypeByIdQuery, Result<PropertyTypeDTO>>, GetPropertyTypeByIdUseCase>();
            services.AddScoped<IRequestHandler<CreatePropertyTypeCommand, Result<Guid>>, CreatePropertyTypeUseCase>();
            services.AddScoped<IRequestHandler<UpdatePropertyTypeCommand, Result<Guid>>, UpdatePropertyTypeUseCase>();
            services.AddScoped<IRequestHandler<DeletePropertyTypeCommand, Result<Guid>>, DeletePropertyTypeUseCase>();

            // Locations - Countries
            services.AddScoped<IRequestHandler<GetCountriesListQuery, Result<PaginationResponse<CountryDTO>>>, GetCountriesListUseCase>();
            services.AddScoped<IRequestHandler<GetCountryByIdQuery, Result<CountryDTO>>, GetCountryByIdUseCase>();
            services.AddScoped<IRequestHandler<CreateCountryCommand, Result<Guid>>, CreateCountryUseCase>();
            services.AddScoped<IRequestHandler<UpdateCountryCommand, Result<Guid>>, UpdateCountryUseCase>();
            services.AddScoped<IRequestHandler<DeleteCountryCommand, Result<Guid>>, DeleteCountryUseCase>();

            // Locations - States
            services.AddScoped<IRequestHandler<GetStatesListQuery, Result<PaginationResponse<StateDTO>>>, GetStatesListUseCase>();
            services.AddScoped<IRequestHandler<GetStateByIdQuery, Result<StateDTO>>, GetStateByIdUseCase>();
            services.AddScoped<IRequestHandler<CreateStateCommand, Result<Guid>>, CreateStateUseCase>();
            services.AddScoped<IRequestHandler<UpdateStateCommand, Result<Guid>>, UpdateStateUseCase>();
            services.AddScoped<IRequestHandler<DeleteStateCommand, Result<Guid>>, DeleteStateUseCase>();

            // Locations - Cities
            services.AddScoped<IRequestHandler<GetCitiesListQuery, Result<PaginationResponse<CityDTO>>>, GetCitiesListUseCase>();
            services.AddScoped<IRequestHandler<GetCityByIdQuery, Result<CityDTO>>, GetCityByIdUseCase>();
            services.AddScoped<IRequestHandler<CreateCityCommand, Result<Guid>>, CreateCityUseCase>();
            services.AddScoped<IRequestHandler<UpdateCityCommand, Result<Guid>>, UpdateCityUseCase>();
            services.AddScoped<IRequestHandler<DeleteCityCommand, Result<Guid>>, DeleteCityUseCase>();

            // Locations - Neighborhoods
            services.AddScoped<IRequestHandler<GetNeighborhoodsListQuery, Result<PaginationResponse<NeighborhoodDTO>>>, GetNeighborhoodsListUseCase>();
            services.AddScoped<IRequestHandler<GetNeighborhoodByIdQuery, Result<NeighborhoodDTO>>, GetNeighborhoodByIdUseCase>();
            services.AddScoped<IRequestHandler<CreateNeighborhoodCommand, Result<Guid>>, CreateNeighborhoodUseCase>();
            services.AddScoped<IRequestHandler<UpdateNeighborhoodCommand, Result<Guid>>, UpdateNeighborhoodUseCase>();
            services.AddScoped<IRequestHandler<DeleteNeighborhoodCommand, Result<Guid>>, DeleteNeighborhoodUseCase>();

            // Validations
            services.AddValidatorsFromAssemblyContaining<CreatePropertyCommandValidator>();

            return services;
        }
    }
}
