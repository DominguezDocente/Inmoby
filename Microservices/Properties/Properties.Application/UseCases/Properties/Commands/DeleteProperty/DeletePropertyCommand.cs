using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.UseCases.Properties.Commands.DeleteProperty
{
    public sealed class DeletePropertyCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }
}
