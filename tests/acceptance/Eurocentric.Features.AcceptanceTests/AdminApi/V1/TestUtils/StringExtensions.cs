namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

internal static class StringExtensions
{
    internal static string ToCountryName(this string countryCode) => countryCode switch
    {
        "AT" => "Austria",
        "BE" => "Belgium",
        "CZ" => "Czechia",
        "DE" => "Germany",
        "ES" => "Spain",
        "FI" => "Finland",
        "GB" => "United Kingdom",
        "HR" => "Croatia",
        "XX" => "Rest of the World",
        _ => "CountryName"
    };
}
