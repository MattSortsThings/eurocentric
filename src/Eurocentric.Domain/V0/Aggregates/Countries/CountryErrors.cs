using Eurocentric.Domain.Functional;

namespace Eurocentric.Domain.V0.Aggregates.Countries;

public static class CountryErrors
{
    public static NotFoundError CountryNotFound(Guid countryId) =>
        new()
        {
            Title = "Country not found",
            Detail = "No country exists with the provided country ID.",
            Metadata = new Dictionary<string, object?> { { nameof(countryId), countryId } },
        };

    public static ConflictError CountryCannotBeDeleted(Guid countryId)
    {
        return new ConflictError
        {
            Title = "Country cannot be deleted",
            Detail = "The country with the provided country ID has a role in one or more contests.",
            Metadata = new Dictionary<string, object?> { { nameof(countryId), countryId } },
        };
    }

    public static ConflictError CountryCodeConflict(string countryCode)
    {
        return new ConflictError
        {
            Title = "Country code conflict",
            Detail = "A country already exists with the provided country code.",
            Metadata = new Dictionary<string, object?> { { nameof(countryCode), countryCode } },
        };
    }

    public static UnprocessableError IllegalCountryCodeValue(string countryCode)
    {
        return new UnprocessableError
        {
            Title = "Illegal country code value",
            Detail = "Country code value must be a string of 2 upper-case letters.",
            Metadata = new Dictionary<string, object?> { { nameof(countryCode), countryCode } },
        };
    }

    public static UnprocessableError IllegalCountryNameValue(string countryName)
    {
        return new UnprocessableError
        {
            Title = "Illegal country name value",
            Detail = "Country name value must be a non-empty, non-whitespace string of no more than 200 characters.",
            Metadata = new Dictionary<string, object?> { { nameof(countryName), countryName } },
        };
    }
}
