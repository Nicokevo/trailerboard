using FluentValidation;
using TrailerBoard.Contracts;

namespace TrailerBoard.Application.Validation;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email).IsValidEmail();
        RuleFor(x => x.Password).IsValidPassword();
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match");
        RuleFor(x => x.FirstName).IsValidName("First name");
        RuleFor(x => x.LastName).IsValidName("Last name");
    }
}
