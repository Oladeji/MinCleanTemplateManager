using MinCleanTemplateManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using MinCleanTemplateManager.Contracts.ResponseDTO.V1;
using LanguageExt;
using DomainErrors;
using CQRSHelper;
using Domain.Interfaces;
namespace MinCleanTemplateManager.Application.CQRS
{
    public class GetSampleModelByGuidQueryHandler : IRequestHandler<GetSampleModelByGuidQuery, Either<GeneralFailure, SampleModelResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetSampleModelByGuidQueryHandler> _logger;
        public ISampleModelRepository _SampleModelRepository;
        public GetSampleModelByGuidQueryHandler(IUnitOfWork unitOfWork, ILogger<GetSampleModelByGuidQueryHandler> logger, ISampleModelRepository SampleModelRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _SampleModelRepository = SampleModelRepository ?? throw new ArgumentNullException(nameof(SampleModelRepository));
        }

        public async Task<Either<GeneralFailure, SampleModelResponseDTO>> Handle(GetSampleModelByGuidQuery request, CancellationToken cancellationToken)
        {
            List<string> includes = new List<string>() { "Models" };
            return (await _SampleModelRepository
                            .GetMatch(s => s.GuidId == request.RequestSampleModelDTO.GuidId, null, cancellationToken))
                            .Map((result) => new SampleModelResponseDTO(result.GuidId, result.SampleModelName, null));
            //.Map((result) => new SampleModelResponseDTO(result.GuidId, result.SampleModelName, convertToModelDto(result.Models)));
        }

        private ICollection<ModelResponseDTO> convertToModelDto(IReadOnlyCollection<Domain.Entities.Model> models)
        {
            return models.Select(x => new ModelResponseDTO(x.GuidId, x.ModelName, x.SampleModelName, null)).ToList();
        }
    }
}