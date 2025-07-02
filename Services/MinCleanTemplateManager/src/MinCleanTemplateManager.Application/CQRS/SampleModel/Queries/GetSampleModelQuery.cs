using MinCleanTemplateManager.Contracts.RequestDTO.V1;
using MinCleanTemplateManager.Contracts.ResponseDTO.V1;
using LanguageExt;

using CQRSHelper;
using DomainErrors;
namespace MinCleanTemplateManager.Application.CQRS
{
    public record GetSampleModelQuery(SampleModelGetRequestDTO RequestSampleModelDTO) : IRequest<Either<GeneralFailure, SampleModelResponseDTO>>;
    public record GetSampleModelByGuidQuery(SampleModelGetRequestByGuidDTO RequestSampleModelDTO) : IRequest<Either<GeneralFailure, SampleModelResponseDTO>>;
    public record GetSampleModelByIdQuery(SampleModelGetRequestByIdDTO RequestSampleModelDTO) : IRequest<Either<GeneralFailure, SampleModelResponseDTO>>;
    public record GetAllSampleModelQuery : IRequest<Either<GeneralFailure, IEnumerable<SampleModelResponseDTO>>>;
}