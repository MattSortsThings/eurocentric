using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountries;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;

public static class ApiDriverExtensions
{
    public static async Task<List<CountryDto>> CreateMultipleCountriesAsync(this IApiDriver driver, params string[] countryCodes)
    {
        List<CountryDto> countries = new(countryCodes.Length);

        foreach (string countryCode in countryCodes)
        {
            CountryDto country = await driver.CreateSingleCountryAsync(countryCode: countryCode);
            countries.Add(country);
        }

        return countries;
    }

    public static async Task<CountryDto> CreateSingleCountryAsync(this IApiDriver driver,
        string countryName = TestDefaults.CountryName,
        string countryCode = "")
    {
        CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = countryName };

        RestRequest request = driver.RequestFactory.Countries.CreateCountry(requestBody);
        ProblemOrResponse<CreateCountryResponse> response = await driver.RestClient.SendAsync<CreateCountryResponse>(request);

        return response.AsResponse.Data!.Country;
    }

    public static async Task<CountryDto[]> GetAllCountriesAsync(this IApiDriver driver)
    {
        RestRequest request = driver.RequestFactory.Countries.GetCountries();
        ProblemOrResponse<GetCountriesResponse> response = await driver.RestClient.SendAsync<GetCountriesResponse>(request);

        return response.AsResponse.Data!.Countries;
    }

    public static async Task DeleteSingleCountryAsync(this IApiDriver driver, Guid countryId) =>
        await driver.BackDoor.ExecuteScopedAsync(DeleteCountryAsync(countryId));

    private static Func<IServiceProvider, Task> DeleteCountryAsync(Guid countryId)
    {
        CountryId countryIdToDelete = CountryId.FromValue(countryId);

        return async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
            await dbContext.Countries.Where(country => country.Id == countryIdToDelete).ExecuteDeleteAsync();
        };
    }
}
