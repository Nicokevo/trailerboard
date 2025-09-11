using FluentValidation;
using TrailerBoard.Contracts;

namespace TrailerBoard.Application.Validation;

public sealed class AddFavoriteRequestValidator : AbstractValidator<FavoriteDto>
{
    public AddFavoriteRequestValidator()
    {
        RuleFor(x => x.PublicId).IsValidPublicId();
    }
}
