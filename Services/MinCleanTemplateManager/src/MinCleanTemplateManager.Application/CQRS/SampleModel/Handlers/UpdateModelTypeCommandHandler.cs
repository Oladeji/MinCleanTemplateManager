using MinCleanTemplateManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using LanguageExt;


using MinCleanTemplateManager.Application.CQRS.SampleModel.Commands;
using CQRSHelper;
using DomainErrors;
using Domain.Interfaces;
namespace MinCleanTemplateManager.Application.CQRS
{
    public class UpdateSampleModelCommandHandler : IRequestHandler<UpdateSampleModelCommand, Either<GeneralFailure, int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateSampleModelCommandHandler> _logger;
        public ISampleModelRepository _SampleModelRepository;
 
        public UpdateSampleModelCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateSampleModelCommandHandler> logger, ISampleModelRepository SampleModelRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _SampleModelRepository = SampleModelRepository ?? throw new ArgumentNullException(nameof(SampleModelRepository));
        }

        public async Task<Either<GeneralFailure, int>> Handle(UpdateSampleModelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("UpdateTestingModeGroupCommandHandler- Request for update not allowed on  {0} it is a primary key", request.UpdateSampleModelDTO.SampleModelName);

            throw new NotImplementedException("Operation Not Allowed ");
           // var entity = Domain.Entities.SampleModel.Create(request.UpdateSampleModelDTO.SampleModelName, request.UpdateSampleModelDTO.SampleModelId);
            //return await _unitOfWork.SampleModelRepository.UpdateAsync(entity, cancellationToken);
            ////_logger.LogInformation("AddNewSampleModelCommandHandler- New data Added");

         //   return await _SampleModelRepository.UpdateAsync(entity, cancellationToken);
        }
    }
}