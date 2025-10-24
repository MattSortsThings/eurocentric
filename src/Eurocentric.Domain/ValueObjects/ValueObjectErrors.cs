using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Domain errors that may occur when instantiating value objects.
/// </summary>
public static class ValueObjectErrors
{
    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="CountryCode" />
    ///     object with an illegal value.
    /// </summary>
    /// <param name="countryCode">The illegal country code value.</param>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalCountryCodeValue(string countryCode)
    {
        return new UnprocessableError
        {
            Title = "Illegal country code value",
            Detail = "Country code value must be a string of 2 upper-case letters.",
            Extensions = new Dictionary<string, object?> { { nameof(countryCode), countryCode } },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="CountryName" />
    ///     object with an illegal value.
    /// </summary>
    /// <param name="countryName">The illegal country name value.</param>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalCountryNameValue(string countryName)
    {
        return new UnprocessableError
        {
            Title = "Illegal country name value",
            Detail = "Country name value must be a non-empty, non-whitespace string of no more than 200 characters.",
            Extensions = new Dictionary<string, object?> { { nameof(countryName), countryName } },
        };
    }
}
