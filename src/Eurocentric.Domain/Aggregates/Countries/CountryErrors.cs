using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Domain errors that may occur when working with <see cref="Country" /> aggregates.
/// </summary>
public static class CountryErrors
{
    /// <summary>
    ///     Creates and returns a new error indicating that the client has requested a <see cref="Country" /> that does not
    ///     exist in the system.
    /// </summary>
    /// <param name="countryId">The ID of the requested country.</param>
    /// <returns>A new <see cref="NotFoundError" /> instance.</returns>
    public static NotFoundError CountryNotFound(CountryId countryId)
    {
        return new NotFoundError
        {
            Title = "Country not found",
            Detail = "The requested country was not found.",
            Extensions = new Dictionary<string, object?> { { nameof(countryId), countryId.Value } },
        };
    }
}
