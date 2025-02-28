using FluentValidation;

namespace INNO.Domain.ViewModels.v1.Professionals.Validations
{
    public class ProfessionalRequestValidation : AbstractValidator<ProfessionalRequestVM>
    {
        public ProfessionalRequestValidation()
        {
            RuleFor(a => a.Name)
                .NotNull()
                .Length(2, 255);

            RuleFor(a => a.Document)
                .NotNull()
                .Length(11);
        }
    }
}
