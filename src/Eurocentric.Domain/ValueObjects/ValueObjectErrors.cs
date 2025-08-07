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
    /// <param name="value">The illegal value that was provided.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalActNameValue(string value) => Error.Failure("Illegal act name value",
        "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.",
        new Dictionary<string, object> { ["actName"] = value });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to instantiate a
    ///     <see cref="BroadcastDate" /> object with an illegal value.
    /// </summary>
    /// <param name="value">The illegal value that was provided.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalBroadcastDateValue(DateOnly value) => Error.Failure("Illegal broadcast date value",
        "Broadcast date value must have a year between 2016 and 2050.",
        new Dictionary<string, object> { ["broadcastDate"] = value });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to instantiate a
    ///     <see cref="CityName" /> object with an illegal value.
    /// </summary>
    /// <param name="value">The illegal value that was provided.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalCityNameValue(string value) => Error.Failure("Illegal city name value",
        "City name value must be a non-empty, non-whitespace string of no more than 200 characters.",
        new Dictionary<string, object> { ["cityName"] = value });

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to instantiate a
    ///     <see cref="ContestYear" /> object with an illegal value.
    /// </summary>
    /// <param name="value">The illegal value that was provided.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalContestYearValue(int value) => Error.Failure("Illegal contest year value",
        "Contest year value must be an integer between 2016 and 2050.",
        new Dictionary<string, object> { ["contestYear"] = value });

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

    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the client has attempted to instantiate a
    ///     <see cref="SongTitle" /> object with an illegal value.
    /// </summary>
    /// <param name="value">The illegal value that was provided.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error IllegalSongTitleValue(string value) => Error.Failure("Illegal song title value",
        "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.",
        new Dictionary<string, object> { ["songTitle"] = value });
}
