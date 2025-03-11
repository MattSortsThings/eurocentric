using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Errors;

public static partial class DomainErrors
{
    public static class Countries
    {
        public static Error CountryNotFound(CountryId countryId) => Error.NotFound("Country not found",
            "No country exists with the specified ID.",
            new Dictionary<string, object> { [nameof(countryId)] = countryId.Value });

        public static Error CountryCodeConflict(Country country) => Error.Conflict("Country code conflict",
            "A country already exists with the specified country code value.",
            new Dictionary<string, object> { ["countryCode"] = country.CountryCode.Value });
    }
}
