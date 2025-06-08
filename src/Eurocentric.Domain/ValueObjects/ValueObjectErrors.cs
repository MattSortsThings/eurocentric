using ErrorOr;

namespace Eurocentric.Domain.ValueObjects;

public static class ValueObjectErrors
{
    public static Error IllegalActNameValue(string value) => Error.Failure("Illegal act name value",
        "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.",
        new Dictionary<string, object> { ["actName"] = value });

    public static Error IllegalBroadcastDateValue(DateOnly value) => Error.Failure("Illegal broadcast date value",
        "Broadcast date value must be between 2016-01-01 and 2050-12-31.",
        new Dictionary<string, object> { ["broadcastDate"] = value });

    public static Error IllegalCityNameValue(string value) => Error.Failure("Illegal city name value",
        "City name value must be a non-empty, non-whitespace string of no more than 200 characters.",
        new Dictionary<string, object> { ["cityName"] = value });

    public static Error IllegalContestYearValue(int value) => Error.Failure("Illegal contest year value",
        "Contest year value must be an integer between 2016 and 2050.",
        new Dictionary<string, object> { ["contestYear"] = value });

    public static Error IllegalCountryCodeValue(string value) => Error.Failure("Illegal country code value",
        "Country code value must be a string of 2 upper-case letters.",
        new Dictionary<string, object> { ["countryCode"] = value });

    public static Error IllegalCountryNameValue(string value) => Error.Failure("Illegal country name value",
        "Country name value must be a non-empty, non-whitespace string of no more than 200 characters.",
        new Dictionary<string, object> { ["countryName"] = value });

    public static Error IllegalSongTitleValue(string value) => Error.Failure("Illegal song title value",
        "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.",
        new Dictionary<string, object> { ["songTitle"] = value });
}
