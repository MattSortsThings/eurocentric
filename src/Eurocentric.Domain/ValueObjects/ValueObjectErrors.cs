using ErrorOr;

namespace Eurocentric.Domain.ValueObjects;

public static class ValueObjectErrors
{
    public static Error IllegalCountryCodeValue(string value) => Error.Failure("Illegal country code value",
        "Country code value must be a string of 2 upper-case letters.",
        new Dictionary<string, object> { ["countryCode"] = value });

    public static Error IllegalCountryNameValue(string value) => Error.Failure("Illegal country name value",
        "Country name value must be a non-empty, non-whitespace string of no more than 200 characters.",
        new Dictionary<string, object> { ["countryName"] = value });
}
