using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.UseCases.Properties.Commands.DeleteProperty
{
    public sealed class DeletePropertyUseCase : BaseHandler, IRequestHandler<DeletePropertyCommand, Result<Guid>>
    {
        private readonly IPropertiesRepository _propertiesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePropertyUseCase(IPropertiesRepository propertiesRepository, IUnitOfWork unitOfWork)
        {
            _propertiesRepository = propertiesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeletePropertyCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                bool deleted = await _propertiesRepository.DeleteWithImagesAsync(command.Id);

                if (!deleted)
                {
                    throw new DirectoryNotFoundException($"Propidad con IDh ID {command.Id} no existe.");
                }

                await _unitOfWork.CommitAsync();

                return command.Id;
            });
        }
    }
}
