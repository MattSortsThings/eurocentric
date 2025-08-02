using Eurocentric.Domain.V0Entities;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Extensions.Countries;

public static class ApiDriverExtensions
{
    public static async Task<Dictionary<string, Guid>> CreateMultipleCountriesAsync(this IApiDriver driver,
        params string[] countryCodes)
    {
        Country[] countries = countryCodes.Select(MapToCountry).ToArray();
        await driver.BackDoor.ExecuteScopedAsync(PersistCountriesAsync(countries));

        return countries.ToDictionary(country => country.CountryCode, country => country.Id);
    }

    private static Country MapToCountry(string countryCode) => new()
    {
        Id = Guid.NewGuid(), CountryCode = countryCode, CountryName = "CountryName", ParticipatingContestIds = []
    };

    private static Func<IServiceProvider, Task> PersistCountriesAsync(Country[] countries)
    {
        Country[] countriesToAdd = countries;

        return async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();

            dbContext.V0Countries.AddRange(countriesToAdd);
            await dbContext.SaveChangesAsync();
        };
    }
}
