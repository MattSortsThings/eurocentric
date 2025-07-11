using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Countries;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using DomainCountry = Eurocentric.Domain.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Contracts.Country;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;

internal static class AdminActorMixins
{
    internal static async Task Given_I_have_created_a_country(this IAdminActor admin,
        string countryName = "",
        string countryCode = "")
    {
        CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = countryName };

        RestRequest request = admin.RequestFactory.Countries.CreateCountry(requestBody);

        ProblemOrResponse<CreateCountryResponse> problemOrResponse =
            await admin.RestClient.SendAsync<CreateCountryResponse>(request, TestContext.Current.CancellationToken);

        admin.GivenCountries.Add(problemOrResponse.AsResponse.Data!.Country);
    }

    internal static async Task Given_I_have_deleted_every_country_I_have_created(this IAdminActor admin)
    {
        HashSet<Guid> countryIds = admin.GivenCountries.Select(x => x.Id).ToHashSet();

        Func<IServiceProvider, Task> deleteCountriesAsync = async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();

            DomainCountry[] countriesToDelete = dbContext.Countries
                .AsEnumerable()
                .Where(country => countryIds.Contains(country.Id.Value))
                .ToArray();

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
}
