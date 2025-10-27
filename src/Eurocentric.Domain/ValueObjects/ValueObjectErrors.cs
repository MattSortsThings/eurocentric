using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Domain errors that may occur when instantiating value objects.
/// </summary>
public static class ValueObjectErrors
{
    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a
    ///     <see cref="BroadcastDate" /> object with an illegal value.
    /// </summary>
    /// <param name="broadcastDate">The illegal broadcast date value.</param>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalBroadcastDateValue(DateOnly broadcastDate)
    {
        return new UnprocessableError
        {
            Title = "Illegal broadcast date value",
            Detail = "Broadcast date value must be a date with a year between 2016 and 2050.",
            Extensions = new Dictionary<string, object?> { { nameof(broadcastDate), broadcastDate } },
        };
    }

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

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a
    ///     <see cref="FinishingPosition" /> object with an illegal value.
    /// </summary>
    /// <param name="finishingPosition">The illegal finishing position value.</param>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalFinishingPositionValue(int finishingPosition)
    {
        return new UnprocessableError
        {
            Title = "Illegal finishing position value",
            Detail = "Finishing position value must be an integer greater than or equal to 1.",
            Extensions = new Dictionary<string, object?> { { nameof(finishingPosition), finishingPosition } },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a
    ///     <see cref="RunningOrderSpot" /> object with an illegal value.
    /// </summary>
    /// <param name="runningOrderSpot">The illegal running order spot value.</param>
    /// <returns>A new <see cref="UnprocessableError" /> instance.</returns>
    public static UnprocessableError IllegalRunningOrderSpotValue(int runningOrderSpot)
    {
        return new UnprocessableError
        {
            Title = "Illegal running order spot value",
            Detail = "Running order spot value must be an integer greater than or equal to 1.",
            Extensions = new Dictionary<string, object?> { { nameof(runningOrderSpot), runningOrderSpot } },
        };
    }
}
