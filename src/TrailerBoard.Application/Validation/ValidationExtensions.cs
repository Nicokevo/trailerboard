using FluentValidation;

namespace TrailerBoard.Application.Validation;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> IsValidEmail<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }

    public static IRuleBuilderOptions<T, string> IsValidPassword<T>(
        this IRuleBuilder<T, string> ruleBuilder, int minLength = 6)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(minLength).WithMessage($"Password must be at least {minLength} characters");
    }

    public static IRuleBuilderOptions<T, string> IsValidName<T>(
        this IRuleBuilder<T, string> ruleBuilder, string fieldName = "Name")
    {
        return ruleBuilder
            .NotEmpty().WithMessage($"{fieldName} is required")
            .MaximumLength(50).WithMessage($"{fieldName} must be at most 50 characters");
    }

    public static IRuleBuilderOptions<T, string> IsValidPublicId<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("PublicId is required")
            .Length(5, 50).WithMessage("PublicId must be between 5 and 50 characters");
    }

    public static IRuleBuilderOptions<T, string> IsValidContent<T>(
        this IRuleBuilder<T, string> ruleBuilder, int maxLength = 500)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Content is required")
            .MaximumLength(maxLength).WithMessage($"Content must be at most {maxLength} characters");
    }

    public static IRuleBuilderOptions<T, string> IsValidUrl<T>(
        this IRuleBuilder<T, string> ruleBuilder, string fieldName = "Url")
    {
        return ruleBuilder
            .NotEmpty().WithMessage($"{fieldName} is required")
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage($"{fieldName} must be a valid URL");
    }
}
