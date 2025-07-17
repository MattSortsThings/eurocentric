using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Countries;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;

public static class AdminActorExtensions
{
    public static async Task Given_I_have_created_a_country(this IAdminActor admin,
        string countryName = "",
        string countryCode = "")
    {
        CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = countryName };

        Country createdCountry = await admin.CreateCountryAsync(requestBody);

        admin.GivenCountries.Add(createdCountry);
    }

    public static async Task Given_I_have_created_some_countries(this IAdminActor admin, params string[] countryCodes)
    {
        admin.GivenCountries.EnsureCapacity(countryCodes.Length);

        Task<Country>[] tasks = countryCodes.Select(MapToCreateCountryRequest)
            .Select(admin.CreateCountryAsync)
            .ToArray();

        Country[] createdCountries = await Task.WhenAll(tasks);

        foreach (Country country in createdCountries)
        {
            admin.GivenCountries.Add(country);
        }
    }

    public static async Task Given_I_have_deleted_my_country(this IAdminActor admin)
    {
        CountryId countryId = CountryId.FromValue(admin.GivenCountries.GetSingle().Id);

        Func<IServiceProvider, Task> deleteAsync = async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();

            await dbContext.Countries.Where(country => country.Id == countryId).ExecuteDeleteAsync();
        };

        await admin.BackDoor.ExecuteScopedAsync(deleteAsync);
    }

    public static async Task Given_I_have_deleted_the_country_with_country_code(this IAdminActor admin, string countryCode)
    {
        CountryId countryId = CountryId.FromValue(admin.GivenCountries.LookupId(countryCode));

        Func<IServiceProvider, Task> deleteAsync = async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();

            await dbContext.Countries.Where(country => country.Id == countryId).ExecuteDeleteAsync();
        };

        await admin.BackDoor.ExecuteScopedAsync(deleteAsync);
    }

    private static async Task<Country> CreateCountryAsync(this IAdminActor admin, CreateCountryRequest requestBody)
    {
        RestRequest request = admin.RequestFactory.Countries.CreateCountry(requestBody);

        ProblemOrResponse<CreateCountryResponse> problemOrResponse =
            await admin.RestClient.SendAsync<CreateCountryResponse>(request, TestContext.Current.CancellationToken);

        return problemOrResponse.AsResponse.Data!.Country;
    }

    private static CreateCountryRequest MapToCreateCountryRequest(string countryCode)
    {
        string countryName = countryCode switch
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
            "NO" => "Norway",
            "XX" => "Rest of the World",
            _ => DefaultValues.CountryName
        };

        return new CreateCountryRequest { CountryCode = countryCode, CountryName = countryName };
    }
}
