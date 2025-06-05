using ErrorOr;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Countries;

public static class CountryErrors
{
    public static Error CountryNotFound(CountryId countryId) => Error.NotFound("Country not found",
        "No country exists with the provided country ID.",
        new Dictionary<string, object> { ["countryId"] = countryId.Value });
}
