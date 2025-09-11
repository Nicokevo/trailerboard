using FluentValidation;
using TrailerBoard.Application.Validation;
using TrailerBoard.Contracts;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).IsValidEmail();
        RuleFor(x => x.Password).IsValidPassword();
    }
}
