using MinCleanTemplateManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using LanguageExt;

using MinCleanTemplateManager.Application.CQRS.SampleModel.Commands;
using DomainErrors;
using CQRSHelper;
using Domain.Interfaces;
namespace MinCleanTemplateManager.Application.CQRS
{
    public class CreateSampleModelCommandHandler : IRequestHandler<CreateSampleModelCommand, Either<GeneralFailure, Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateSampleModelCommandHandler> _logger;
        public ISampleModelRepository _SampleModelRepository;
        public CreateSampleModelCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateSampleModelCommandHandler> logger, ISampleModelRepository SampleModelRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _SampleModelRepository = SampleModelRepository ?? throw new ArgumentNullException(nameof(SampleModelRepository));
        }

        public async Task<Either<GeneralFailure, Guid>> Handle(CreateSampleModelCommand request, CancellationToken cancellationToken)
        {
            var entity = Domain.Entities.SampleModel.Create(request.CreateSampleModelDTO.SampleModelName, request.CreateSampleModelDTO.GuidId);
            var pp = await _SampleModelRepository.AddAsync(entity, cancellationToken);

            var xx = (pp).Map((x) => entity.GuidId);
            return xx;
            // return (await _unitOfWork.SampleModelRepository.AddAsync(entity, cancellationToken)).Map((x) => entity.GuidId);
            //throw new NotImplementedException();
            //Follow the format below , initial the entity variable by calling the entity Create method;
        }//var entity =null; Domain.Entities.SampleModel.Create(request.SampleModelCreateDTO.SampleModelName, request.SampleModelCreateDTO.Value.GuidId);return ( await  _SampleModelRepository.AddAsync(entity, cancellationToken)). Map((x) =>  entity.GuidId);
    }
}