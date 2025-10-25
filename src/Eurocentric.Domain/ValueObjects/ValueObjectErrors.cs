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
    ///     Creates and returns a new error indicating that the client has attempted to create an <see cref="ActName" />
    ///     object with an illegal value.
    /// </summary>
    /// <param name="actName">The illegal act name value.</param>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalActNameValue(string actName)
    {
        return new UnprocessableError
        {
            Title = "Illegal act name value",
            Detail = "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.",
            Extensions = new Dictionary<string, object?> { { nameof(actName), actName } },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="CityName" />
    ///     object with an illegal value.
    /// </summary>
    /// <param name="cityName">The illegal city name value.</param>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalCityNameValue(string cityName)
    {
        return new UnprocessableError
        {
            Title = "Illegal city name value",
            Detail = "City name value must be a non-empty, non-whitespace string of no more than 200 characters.",
            Extensions = new Dictionary<string, object?> { { nameof(cityName), cityName } },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="SongTitle" />
    ///     object with an illegal value.
    /// </summary>
    /// <param name="songTitle">The illegal song title value.</param>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalSongTitleValue(string songTitle)
    {
        return new UnprocessableError
        {
            Title = "Illegal song title value",
            Detail = "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.",
            Extensions = new Dictionary<string, object?> { { nameof(songTitle), songTitle } },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="ContestYear" />
    ///     object with an illegal value.
    /// </summary>
    /// <param name="contestYear">The illegal contest year value.</param>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalContestYearValue(int contestYear)
    {
        return new UnprocessableError
        {
            Title = "Illegal contest year value",
            Detail = "Contest year value must be an integer between 2016 and 2050.",
            Extensions = new Dictionary<string, object?> { { nameof(contestYear), contestYear } },
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
