using Eurocentric.Domain.Errors;

namespace Eurocentric.Domain.Aggregates.Placeholders;

public static class CountryErrors
{
    public static DomainError CountryNotFound(Guid countryId)
    {
        return new DomainError
        {
            Type = DomainErrorType.NotFound,
            Title = "Country not found",
            Description = "The requested country does not exist.",
            AdditionalData = new Dictionary<string, object?> { { nameof(countryId), countryId } },
        };
    }

    public static DomainError IllegalCountryCodeValue(string countryCode)
    {
        return new DomainError
        {
            Type = DomainErrorType.Intrinsic,
            Title = "Illegal country code value",
            Description = "Country code value must be a string of 2 upper-case ASCII letters.",
            AdditionalData = new Dictionary<string, object?> { { nameof(countryCode), countryCode } },
        };
    }

    public static DomainError IllegalCountryNameValue(string countryName)
    {
        return new DomainError
        {
            Type = DomainErrorType.Intrinsic,
            Title = "Illegal country name value",
            Description =
                "Country name value must be a non-empty, non-whitespace string that does not start "
                + "or end with whitespace and does not contain a line break.",
            AdditionalData = new Dictionary<string, object?> { { nameof(countryName), countryName } },
        };
    }

    public static DomainError CountryCodeConflict(string countryCode)
    {
        return new DomainError
        {
            Type = DomainErrorType.Extrinsic,
            Title = "Country code conflict",
            Description = "A country already exists with the requested country code.",
            AdditionalData = new Dictionary<string, object?> { { nameof(countryCode), countryCode } },
        };
    }

    public static DomainError CountryDeletionDisallowed(Guid countryId)
    {
        return new DomainError
        {
            Type = DomainErrorType.Extrinsic,
            Title = "Country deletion disallowed",
            Description = "The requested country cannot be deleted because it has one or more contest roles.",
            AdditionalData = new Dictionary<string, object?> { { nameof(countryId), countryId } },
        };
    }
}
