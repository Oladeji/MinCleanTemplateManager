using Microsoft.Extensions.Logging;
using MinCleanTemplateManager.Contracts.ResponseDTO.V1;
using LanguageExt;
using DomainErrors;
using Domain.Interfaces;
using CQRSHelper;
using MinCleanTemplateManager.Domain.Interfaces;
namespace MinCleanTemplateManager.Application.CQRS
{
    public class GetSampleModelByIdQueryHandler : IRequestHandler<GetSampleModelByIdQuery, Either<GeneralFailure, SampleModelResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetSampleModelByIdQueryHandler> _logger;
        public ISampleModelRepository _SampleModelRepository;
        public GetSampleModelByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetSampleModelByIdQueryHandler> logger, ISampleModelRepository SampleModelRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _SampleModelRepository = SampleModelRepository ?? throw new ArgumentNullException(nameof(SampleModelRepository));
        }

        public async Task<Either<GeneralFailure, SampleModelResponseDTO>> Handle(GetSampleModelByIdQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException("Not Implemented");
            List<string> includes = new List<string>() { "Models" };
            return (await _SampleModelRepository
                            //==4
                            //.GetMatch(s => s.SampleModelName == request.SampleModelRequestDTO.Value.SampleModelId, includes, cancellationToken))
                            //.Map((result) => new ApplicationSampleModelResponseDTO(result.GuidId, result.SampleModelName, convertToModelDto(result.Models)));

                            .GetMatch(s => s.SampleModelName == request.RequestSampleModelDTO.SampleModelId, null, cancellationToken))
                            .Map((result) => new SampleModelResponseDTO(result.GuidId, result.SampleModelName, null));
            //.Map((result) => new SampleModelResponseDTO(result.GuidId, result.SampleModelName, convertToModelDto(result.Models)));
        }

        private ICollection<ModelResponseDTO> convertToModelDto(IReadOnlyCollection<Domain.Entities.Model> models)
        {
            return models.Select(x => new ModelResponseDTO(x.GuidId, x.ModelName, x.SampleModelName, null)).ToList();
        }
    }
}