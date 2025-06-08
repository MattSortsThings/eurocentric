using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Countries;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

internal static partial class AdminApiV1DriverExtensions
{
    internal static async Task<Country> CreateACountryAsync(this IAdminApiV1Driver.ICountries driver,
        string? countryName = null,
        string countryCode = "",
        CancellationToken cancellationToken = default)
    {
        CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = countryName ?? "CountryName" };

        ProblemOrResponse<CreateCountryResponse> problemOrResponse = await driver.CreateCountry(requestBody, cancellationToken);

        return problemOrResponse.AsResponse.Data!.Country;
    }

    internal static async Task<Country[]> CreateMultipleCountriesAsync(this IAdminApiV1Driver.ICountries driver,
        IEnumerable<string> countryCodes,
        CancellationToken cancellationToken = default)
    {
        Task<Country>[] tasks = countryCodes.Select(countryCode =>
                driver.CreateACountryAsync(GetCountryName(countryCode), countryCode, cancellationToken))
            .ToArray();

        return await Task.WhenAll(tasks);
    }

    internal static async Task<Country> GetACountryAsync(this IAdminApiV1Driver.ICountries driver,
        Guid countryId,
        CancellationToken cancellationToken = default)
    {
        ProblemOrResponse<GetCountryResponse> problemOrResponse = await driver.GetCountry(countryId, cancellationToken);

        return problemOrResponse.AsResponse.Data!.Country;
    }

    internal static async Task<Country[]> GetAllCountriesAsync(this IAdminApiV1Driver.ICountries driver,
        CancellationToken cancellationToken = default)
    {
        ProblemOrResponse<GetCountriesResponse> problemOrResponse = await driver.GetCountries(cancellationToken);

        return problemOrResponse.AsResponse.Data!.Countries;
    }

    private static string GetCountryName(string countryCode) => countryCode switch
    {
        "AT" => "Austria",
        "BE" => "Belgium",
        "CZ" => "Czechia",
        "DK" => "Denmark",
        "EE" => "Estonia",
        "FI" => "Finland",
        "GB" => "United Kingdom",
        "HR" => "Croatia",
        "IT" => "Italy",
        "XX" => "Rest of the World",
        _ => "CountryName"
    };
}
