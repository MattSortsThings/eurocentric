using ErrorOr;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Factory methods to create common country aggregate errors.
/// </summary>
public static class CountryErrors
{
    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the requested country aggregate was not found.
    /// </summary>
    /// <param name="countryId">The requested country ID.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error CountryNotFound(CountryId countryId) => Error.NotFound("Country not found",
        "No country exists with the provided country ID.",
        new Dictionary<string, object> { { "countryId", countryId.Value } });
}
