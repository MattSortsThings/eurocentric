using ErrorOr;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Countries;

public static class CountryErrors
{
    public static Error CountryCodeConflictError(CountryCode countryCode) => Error.Conflict("Country code conflict",
        "Country already exists with the provided country code.",
        new Dictionary<string, object> { { "countryCode", countryCode.Value } });
}
