using ErrorOr;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Factory methods to create common value object type errors.
/// </summary>
public static class ValueObjectErrors
{
    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to instantiate a
    ///     <see cref="ActName" /> object with an illegal value.
    /// </summary>
    /// <param name="actName">The illegal act name value that was provided.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalActNameValue(string actName) => Error.Failure("Illegal act name value",
        "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.",
        new Dictionary<string, object> { { nameof(actName), actName } });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to instantiate a
    ///     <see cref="CityName" /> object with an illegal value.
    /// </summary>
    /// <param name="cityName">The illegal city name value that was provided.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalCityNameValue(string cityName) => Error.Failure("Illegal city name value",
        "City name value must be a non-empty, non-whitespace string of no more than 200 characters.",
        new Dictionary<string, object> { { nameof(cityName), cityName } });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to instantiate a
    ///     <see cref="ContestYear" /> object with an illegal value.
    /// </summary>
    /// <param name="contestYear">The illegal contest year value that was provided.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalContestYearValue(int contestYear) => Error.Failure("Illegal contest year value",
        "Contest year value must be an integer between 2016 and 2050.",
        new Dictionary<string, object> { { "contestYear", contestYear } });

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

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to instantiate a
    ///     <see cref="SongTitle" /> object with an illegal value.
    /// </summary>
    /// <param name="songTitle">The illegal song title value that was provided.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalSongTitleValue(string songTitle) => Error.Failure("Illegal song title value",
        "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.",
        new Dictionary<string, object> { { nameof(songTitle), songTitle } });
}
