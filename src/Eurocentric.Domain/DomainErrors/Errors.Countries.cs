using ErrorOr;

namespace Eurocentric.Domain.DomainErrors;

public static class Errors
{
    public static class Countries
    {
        public static Error CountryNotFound(Guid countryId) => Error.NotFound("Country not found",
            "No country exists with the specified ID.",
            new Dictionary<string, object> { [nameof(countryId)] = countryId });

        public static Error CountryCodeConflict(string countryCode) => Error.Conflict("Country code conflict",
            "A country already exists with the specified country code value.",
            new Dictionary<string, object> { [nameof(countryCode)] = countryCode });

        public static Error InvalidCountryCode(string countryCode) => Error.Failure("Invalid country code",
            "Country code value must be a string of 2 upper-case letters.",
            new Dictionary<string, object> { [nameof(countryCode)] = countryCode });

        public static Error InvalidCountryName(string countryName) => Error.Failure("Invalid country name",
            "Country name value must be a non-empty, non-white-space string of no more than 200 characters.",
            new Dictionary<string, object> { [nameof(countryName)] = countryName });
    }
}
