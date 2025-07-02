using MinCleanTemplateManager.Application.CQRS.SampleModel.Commands;
using FluentValidation;
using CQRSHelper;

using LanguageExt;
using DomainErrors;

namespace MinCleanTemplateManager.Application.Behaviours
{
    public class ValidationSampleModelBehaviour : IPipelineBehavior<CreateSampleModelCommand, Either<GeneralFailure, Guid>>
    {
        private readonly IValidator<CreateSampleModelCommand> _validator;

        public ValidationSampleModelBehaviour(IValidator<CreateSampleModelCommand> validator)
        {
            _validator = validator;
        }

        public async Task<Either<GeneralFailure, Guid>> Handle(CreateSampleModelCommand request, RequestHandlerDelegate<Either<GeneralFailure, Guid>> next, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid)
            {
                return await next();
            }
            var errors = new List<string>();
            //var errors = validationResult.Errors
            //    .Select(error => new ValidationError(error.PropertyName, error.ErrorMessage))
            //    .ToList();

            return new GeneralFailure("ValidationError", "errors.First().ErrorMessage","errors.First().PropertyName", FailureType.BadRequestFailure);
        }
    }
}
