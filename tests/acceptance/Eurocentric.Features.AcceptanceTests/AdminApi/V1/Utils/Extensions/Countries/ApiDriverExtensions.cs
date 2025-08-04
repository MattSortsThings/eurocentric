using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CountryAggregate = Eurocentric.Domain.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;

public static class ApiDriverExtensions
{
    public static async Task<CountryDto> CreateSingleCountryAsync(this IApiDriver driver, string countryName = "CountryName",
        string countryCode = "")
    {
        CountryAggregate country = new(CountryId.FromValue(Guid.NewGuid()),
            CountryCode.FromValue(countryCode).Value,
            CountryName.FromValue(countryName).Value);

        await driver.BackDoor.ExecuteScopedAsync(PersistCountryAsync(country));

        return country.ToCountryDto();
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

    private static Func<IServiceProvider, Task> PersistCountryAsync(CountryAggregate country)
    {
        CountryAggregate countryToAdd = country;

        return async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
            dbContext.Countries.Add(countryToAdd);
            await dbContext.SaveChangesAsync();
        };
    }

    private static CountryDto ToCountryDto(this CountryAggregate country) => new()
    {
        Id = country.Id.Value,
        CountryCode = country.CountryCode.Value,
        CountryName = country.CountryName.Value,
        ParticipatingContestIds = country.ParticipatingContestIds.Select(id => id.Value).ToArray()
    };
}
