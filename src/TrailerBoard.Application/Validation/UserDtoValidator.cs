using FluentValidation;
using TrailerBoard.Contracts;

namespace TrailerBoard.Application.Validation;

public sealed class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.Email).IsValidEmail();
        RuleFor(x => x.FirstName).IsValidName("First name");
        RuleFor(x => x.LastName).IsValidName("Last name");
    }
}
