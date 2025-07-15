using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Countries;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using DomainCountry = Eurocentric.Domain.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Contracts.Country;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;

internal static class AdminActorMixins
{
    internal static async Task Given_I_have_created_some_countries(this IAdminActor admin, params string[] countryCodes)
    {
        admin.GivenCountries.EnsureCapacity(countryCodes.Length);

        foreach (string countryCode in countryCodes)
        {
            await admin.Given_I_have_created_a_country(countryCode: countryCode, countryName: GetCountryName(countryCode));
        }
    }

    internal static async Task Given_I_have_created_a_country(this IAdminActor admin,
        string countryName = "",
        string countryCode = "")
    {
        CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = countryName };

        RestRequest request = admin.RequestFactory.Countries.CreateCountry(requestBody);

        ProblemOrResponse<CreateCountryResponse> problemOrResponse =
            await admin.RestClient.SendAsync<CreateCountryResponse>(request, TestContext.Current.CancellationToken);

        CountryDto createdCountry = problemOrResponse.AsResponse.Data!.Country;

        admin.GivenCountries.Add(createdCountry.CountryCode, createdCountry);
    }

    internal static async Task Given_I_have_deleted_the_country(this IAdminActor admin, string countryCode)
    {
        CountryId countryId = CountryId.FromValue(admin.GivenCountries.GetId(countryCode));

        Func<IServiceProvider, Task> deleteCountryAsync = async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();

            await dbContext.Countries.Where(country => country.Id.Equals(countryId)).ExecuteDeleteAsync();
        };

        await admin.BackDoor.ExecuteScopedAsync(deleteCountryAsync);
    }

    internal static async Task Given_I_have_deleted_every_country_I_have_created(this IAdminActor admin)
    {
        HashSet<CountryId> countryIds = admin.GivenCountries.GetAllCountries()
            .Select(x => x.Id)
            .Select(CountryId.FromValue)
            .ToHashSet();

        Func<IServiceProvider, Task> deleteCountriesAsync = async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();

            IEnumerable<DomainCountry> countriesToDelete = dbContext.Countries.AsSplitQuery()
                .AsEnumerable()
                .Join(countryIds,
                    x => x.Id,
                    y => y,
                    (x, _) => x);

            dbContext.Countries.RemoveRange(countriesToDelete);

            await dbContext.SaveChangesAsync();
        };

        await admin.BackDoor.ExecuteScopedAsync(deleteCountriesAsync);
    }

    private static CountryDto MapToCountryDto(DomainCountry country) => new()
    {
        Id = country.Id.Value,
        CountryCode = country.CountryCode.Value,
        CountryName = country.CountryName.Value,
        ParticipatingContestIds = country.ParticipatingContestIds.Select(id => id.Value).ToArray()
    };

    private static string GetCountryName(string countryCode) => countryCode switch
    {
        "AT" => "Austria",
        "BE" => "Belgium",
        "CZ" => "Czechia",
        "DK" => "Denmark",
        "EE" => "Estonia",
        "FI" => "Finland",
        "GE" => "Georgia",
        "GB" => "United Kingdom",
        "IT" => "Italy",
        "NO" => "Norway",
        "XX" => "Rest of the World",
        _ => "CountryName"
    };
}
