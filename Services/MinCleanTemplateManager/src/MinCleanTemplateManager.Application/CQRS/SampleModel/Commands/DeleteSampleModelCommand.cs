using MinCleanTemplateManager.Contracts.RequestDTO.V1;
using LanguageExt;
using CQRSHelper;
using DomainErrors;
namespace MinCleanTemplateManager.Application.CQRS.SampleModel.Commands
{
    public  record DeleteSampleModelCommand(SampleModelDeleteRequestDTO  DeleteSampleModelDTO) :  IRequest<Either<GeneralFailure, int>>;
}