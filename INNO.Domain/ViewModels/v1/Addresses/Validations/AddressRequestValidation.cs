using FluentValidation;

namespace INNO.Domain.ViewModels.v1.Addresses.Validations
{
    public class AddressRequestValidation : AbstractValidator<AddressRequestVM>
    {
        public AddressRequestValidation()
        {
            RuleFor(a => a.State)
                .Length(2);
        }
    }
}
