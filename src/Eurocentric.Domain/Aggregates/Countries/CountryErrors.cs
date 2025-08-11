using ErrorOr;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

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

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to create a country aggregate
    ///     with a non-unique country code.
    /// </summary>
    /// <param name="countryCode">The country code.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error CountryCodeConflict(CountryCode countryCode) => Error.Conflict("Country code conflict",
        "A country already exists with the provided country code.",
        new Dictionary<string, object> { { "countryCode", countryCode.Value } });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to build a country without
    ///     setting the country code.
    /// </summary>
    /// <returns></returns>
    public static Error CountryCodeNotSet() => Error.Unexpected("Country code not set",
        "Country builder invoked without setting country code.");

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to build a country without
    ///     setting the country name.
    /// </summary>
    /// <returns></returns>
    public static Error CountryNameNotSet() => Error.Unexpected("Country name not set",
        "Country builder invoked without setting country name.");

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to delete a country aggregate
    ///     that participates in one or more contests.
    /// </summary>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error CountryDeletionBlocked() => Error.Conflict("Country deletion blocked",
        "The country cannot be deleted because it participates in one or more contests.");
}
