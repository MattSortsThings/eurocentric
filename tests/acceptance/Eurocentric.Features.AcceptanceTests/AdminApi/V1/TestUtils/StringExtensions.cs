namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

internal static class StringExtensions
{
    internal static string ToCountryName(this string countryCode) => countryCode switch
    {
        "AT" => "Austria",
        "BE" => "Belgium",
        "CZ" => "Czechia",
        "DE" => "Germany",
        "EE" => "Spain",
        "ES" => "Spain",
        "FI" => "Finland",
        "GB" => "United Kingdom",
        "HR" => "Croatia",
        "IT" => "Italy",
        "XX" => "Rest of the World",
        _ => "CountryName"
    };
}
