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
    /// <param name="value">The illegal value that was provided.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalCountryCodeValue(string value) => Error.Failure("Illegal country code value",
        "Country code value must be a string of 2 upper-case letters.",
        new Dictionary<string, object> { ["countryCode"] = value });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to instantiate a
    ///     <see cref="CountryName" /> object with an illegal value.
    /// </summary>
    /// <param name="value">The illegal value that was provided.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalCountryNameValue(string value) => Error.Failure("Illegal country name value",
        "Country name value must be a non-empty, non-whitespace string of no more than 200 characters.",
        new Dictionary<string, object> { ["countryName"] = value });
}
