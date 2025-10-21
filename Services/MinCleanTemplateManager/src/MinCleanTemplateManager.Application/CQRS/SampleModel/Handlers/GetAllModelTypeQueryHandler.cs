using MinCleanTemplateManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using MinCleanTemplateManager.Contracts.ResponseDTO.V1;
using LanguageExt;
using CQRSHelper;
using DomainErrors;
using Domain.Interfaces;

namespace MinCleanTemplateManager.Application.CQRS
{
    public class GetAllSampleModelQueryHandler : IRequestHandler<GetAllSampleModelQuery, Either<GeneralFailure, IEnumerable<SampleModelResponseDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllSampleModelQueryHandler> _logger;
        public ISampleModelRepository _SampleModelRepository;
        public GetAllSampleModelQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllSampleModelQueryHandler> logger, ISampleModelRepository SampleModelRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _SampleModelRepository = SampleModelRepository ?? throw new ArgumentNullException(nameof(SampleModelRepository));
        }

        public async Task<Either<GeneralFailure, IEnumerable<SampleModelResponseDTO>>> Handle(GetAllSampleModelQuery request, CancellationToken cancellationToken)
        {
            return (await _SampleModelRepository    
           .GetAllAsync(s => true, null, null,null, cancellationToken))
           .Map(task => task
          .Select(result => new SampleModelResponseDTO(result.GuidId, result.SampleModelName, null)));
        }
    }
}