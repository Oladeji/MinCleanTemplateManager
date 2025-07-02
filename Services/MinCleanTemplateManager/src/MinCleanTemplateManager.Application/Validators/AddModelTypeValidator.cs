using MinCleanTemplateManager.Application.CQRS.SampleModel.Commands;
using FluentValidation;

namespace MinCleanTemplateManager.Application.Validators
{
    public class AddSampleModelValidator : AbstractValidator<CreateSampleModelCommand>
    {
        public AddSampleModelValidator()
        {
            RuleFor(x => x.CreateSampleModelDTO).NotNull().NotEmpty();
            // RuleFor(x=>x.SampleModelsName)..NotEmpty();
        }
    }
}
