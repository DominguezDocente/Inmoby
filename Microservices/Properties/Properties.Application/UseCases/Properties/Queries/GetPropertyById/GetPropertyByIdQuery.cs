using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.UseCases.Properties.Queries.GetPropertyById
{
    public sealed class GetPropertyByIdQuery : IRequest<Result<PropertyDetailsDTO>>
    {
        public Guid Id { get; set; }
    }
}
