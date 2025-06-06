using ErrorOr;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Countries;

public static class CountryErrors
{
    public static Error CountryNotFound(CountryId countryId) => Error.NotFound("Country not found",
        "No country exists with the provided country ID.",
        new Dictionary<string, object> { ["countryId"] = countryId.Value });

    public static Error CountryCodeConflict(CountryCode countryCode) => Error.Conflict("Country code conflict",
        "A country already exists with the provided country code.",
        new Dictionary<string, object> { ["countryCode"] = countryCode.Value });
}
