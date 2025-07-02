
using CQRSHelper;
using DomainErrors;
using LanguageExt;
using MinCleanTemplateManager.Contracts.RequestDTO.V1;

namespace MinCleanTemplateManager.Application.CQRS.SampleModel.Commands
{
    public record CreateSampleModelCommand(SampleModelCreateRequestDTO CreateSampleModelDTO) : IRequest<Either<GeneralFailure, Guid>>;
}