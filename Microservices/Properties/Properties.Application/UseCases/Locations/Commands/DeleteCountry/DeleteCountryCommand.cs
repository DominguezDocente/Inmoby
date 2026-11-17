using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Commands.DeleteCountry
{
    public sealed class DeleteCountryCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }

    public sealed class DeleteCountryUseCase : BaseHandler, IRequestHandler<DeleteCountryCommand, Result<Guid>>
    {
        private readonly ICountriesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCountryUseCase(ICountriesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeleteCountryCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                Country? country = await _repository.GetByIdAsync(command.Id);

                if (country is null)
                {
                    throw new NotFoundException($"No existe un país con ID {command.Id}.");
                }

                await _repository.DeleteAsync(command.Id);
                await _unitOfWork.CommitAsync();

                return command.Id;
            });
        }
    }
}
