using MinCleanTemplateManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using LanguageExt;

using MinCleanTemplateManager.Application.CQRS.SampleModel.Commands;
using CQRSHelper;
using DomainErrors;
using Domain.Interfaces;
namespace MinCleanTemplateManager.Application.CQRS
{
    public class DeleteSampleModelCommandHandler : IRequestHandler<DeleteSampleModelCommand, Either<GeneralFailure, int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ISampleModelRepository _SampleModelRepository;
        private readonly ILogger<DeleteSampleModelCommandHandler> _logger;
        public DeleteSampleModelCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteSampleModelCommandHandler> logger, ISampleModelRepository SampleModelRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _SampleModelRepository = SampleModelRepository ?? throw new ArgumentNullException(nameof(SampleModelRepository));
        }

        public async Task<Either<GeneralFailure, int>> Handle(DeleteSampleModelCommand request, CancellationToken cancellationToken)
        {
            //return (
            //    await _unitOfWork.SampleModelRepository
            //    .GetMatch(s => (s.GuidId.Equals(request.DeleteSampleModelDTO.guid)), null, cancellationToken))
            //    .Match(Left: x => x, Right: x => _unitOfWork.SampleModelRepository
            //    .DeleteAsync(x, cancellationToken)
            //    .Result);
            return await _SampleModelRepository.DeleteByGuidAsync(request.DeleteSampleModelDTO.guid, cancellationToken);


        }
    }
}