using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.ValueObjects;
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
