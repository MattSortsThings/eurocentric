using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CountryAggregate = Eurocentric.Domain.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Apis.Admin.V1.Dtos.Countries.Country;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public static class Shortcuts
{
    public static async Task<CountryDto> CreateACountryAsync(
        this AdminKernel kernel,
        string countryName = "",
        string countryCode = ""
    )
    {
        CountryAggregate aggregate = new(
            CountryId.FromValue(Guid.NewGuid()),
            CountryCode.FromValue(countryCode).GetValueOrDefault(),
            CountryName.FromValue(countryName).GetValueOrDefault()
        );

        CountryDto dto = aggregate.ToDto();

        await kernel.BackDoor.ExecuteScopedAsync(PersistAsync(aggregate));

        return dto;
    }

    public static async Task DeleteACountryAsync(this AdminKernel kernel, Guid countryId)
    {
        CountryId id = CountryId.FromValue(countryId);
        await kernel.BackDoor.ExecuteScopedAsync(DeleteAsync(id));
    }

    private static Func<IServiceProvider, Task> DeleteAsync(CountryId countryId)
    {
        CountryId idToDelete = countryId;

        return async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
            await dbContext.Countries.Where(country => country.Id.Equals(idToDelete)).ExecuteDeleteAsync();
        };
    }

    private static Func<IServiceProvider, Task> PersistAsync(CountryAggregate aggregate)
    {
        CountryAggregate aggregateToPersist = aggregate;

        return async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
            dbContext.Countries.Add(aggregateToPersist);
            await dbContext.SaveChangesAsync();
        };
    }
}
