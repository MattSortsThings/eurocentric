using ErrorOr;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Factory methods to create common value object type errors.
/// </summary>
public static class ValueObjectErrors
{
    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to instantiate a
    ///     <see cref="CountryCode" /> object with an illegal value.
    /// </summary>
    /// <param name="countryCode">The illegal country code value that was provided.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalCountryCodeValue(string countryCode) => Error.Failure("Illegal country code value",
        "Country code value must be a string of 2 upper-case letters.",
        new Dictionary<string, object> { { nameof(countryCode), countryCode } });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to instantiate a
    ///     <see cref="CountryName" /> object with an illegal value.
    /// </summary>
    /// <param name="countryName">The illegal country name value that was provided.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalCountryNameValue(string countryName) => Error.Failure("Illegal country name value",
        "Country name value must be a non-empty, non-whitespace string of no more than 200 characters.",
        new Dictionary<string, object> { { nameof(countryName), countryName } });
}
