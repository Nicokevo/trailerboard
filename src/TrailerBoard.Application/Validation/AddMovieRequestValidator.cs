using FluentValidation;
using TrailerBoard.Contracts;

namespace TrailerBoard.Application.Validation;

public sealed class CreateMovieRequestValidator : AbstractValidator<CreateMovieRequest>
{
    public CreateMovieRequestValidator()
    {
        RuleFor(x => x.PublicId).IsValidPublicId();
        RuleFor(x => x.Title).IsValidName("Title");
        RuleFor(x => x.Year)
            .InclusiveBetween(1900, DateTime.UtcNow.Year)
            .WithMessage("Year must be valid");
        RuleFor(x => x.PosterUrl).IsValidUrl("PosterUrl");
        RuleFor(x => x.TrailerUrl).IsValidUrl("TrailerUrl");
    }
}
