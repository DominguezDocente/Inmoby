using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.UseCases.Properties.Queries.GetPropertyById
{
    public sealed class GetPropertyByIdUseCase : BaseHandler, IRequestHandler<GetPropertyByIdQuery, Result<PropertyDetailsDTO>>
    {
        private readonly IPropertiesRepository _propertiesRepository;

        public GetPropertyByIdUseCase(IPropertiesRepository propertiesRepository)
        {
            _propertiesRepository = propertiesRepository;
        }

        public async Task<Result<PropertyDetailsDTO>> Handle(GetPropertyByIdQuery query)
        {

            return await ExecuteAsync(async () =>
            {
                Property? property = await _propertiesRepository.GetByIdWithDetailsAsync(query.Id);

                if (property is null)
                {
                    throw new NotFoundException($"No existe una propiedad con el ID {query.Id}");
                }

                return property.ToDetailsDTO();
            });
        }
    }
}
