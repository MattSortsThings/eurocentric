namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries.TestUtils;

public static class StringExtension
{
    public static string ToCountryName(this string countryCode) => countryCode switch
    {
        "AT" => "Austria",
        "BE" => "Belgium",
        "CZ" => "Czechia",
        "DE" => "Germany",
        "EE" => "Estonia",
        "FI" => "Finland",
        "GB" => "United Kingdom",
        "HR" => "Croatia",
        "XX" => "Rest of the World",
        _ => "DefaultCountryName"
    };
}
