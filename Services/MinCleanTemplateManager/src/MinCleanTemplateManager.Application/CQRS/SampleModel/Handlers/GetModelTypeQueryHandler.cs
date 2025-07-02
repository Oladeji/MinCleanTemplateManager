using MinCleanTemplateManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using MinCleanTemplateManager.Contracts.ResponseDTO.V1;

using LanguageExt;

using CQRSHelper;
using DomainErrors;
using Domain.Interfaces;
namespace MinCleanTemplateManager.Application.CQRS
{
    public class GetSampleModelQueryHandler : IRequestHandler<GetSampleModelQuery, Either<GeneralFailure, SampleModelResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetSampleModelQueryHandler> _logger;
        public ISampleModelRepository _SampleModelRepository;
        public GetSampleModelQueryHandler(IUnitOfWork unitOfWork, ILogger<GetSampleModelQueryHandler> logger, ISampleModelRepository SampleModelRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _SampleModelRepository = SampleModelRepository ?? throw new ArgumentNullException(nameof(SampleModelRepository));
        }

        public async Task<Either<GeneralFailure, SampleModelResponseDTO>> Handle(GetSampleModelQuery request, CancellationToken cancellationToken)
        {
            //  List<string> includes = new List<string>() { "Models" };
            return (await _SampleModelRepository
                    .GetMatch(s => (s.SampleModelName == request.RequestSampleModelDTO.SampleModelName), null, cancellationToken))
                    .Map((result) => new SampleModelResponseDTO(result.GuidId, result.SampleModelName, null));
            //.Map((result) => new SampleModelResponseDTO(result.GuidId, result.SampleModelName, convertToModelDto(result.Models)));

        }

        private ICollection<ModelResponseDTO> convertToModelDto(IEnumerable<Domain.Entities.Model> models)
        {
            return models.Select(x => new ModelResponseDTO(x.GuidId, x.ModelName, x.SampleModelName, null)).ToList();
        }
    }
}