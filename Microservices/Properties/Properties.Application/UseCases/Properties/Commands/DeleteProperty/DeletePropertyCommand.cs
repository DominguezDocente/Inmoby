using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;

namespace Properties.Application.UseCases.Properties.Commands.DeleteProperty;

public sealed class DeletePropertyCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}
