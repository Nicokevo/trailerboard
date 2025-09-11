using FluentValidation;
using TrailerBoard.Contracts;

namespace TrailerBoard.Application.Validation;

public sealed class AddCommentRequestValidator : AbstractValidator<AddCommentRequest>
{
    public AddCommentRequestValidator()
    {
        RuleFor(x => x.Content).IsValidContent();
    }
}
