using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V0.Countries.TestUtils;

public static class CountryBackDoorOperations
{
    public static Func<IServiceProvider, Task> AddFakeContestRoleToCountry(Guid countryId)
    {
        CountryId targetCountryId = CountryId.FromValue(countryId);
        ContestId fakeContestId = ContestId.FromValue(Guid.Parse("3d764835-1da8-4d52-8a93-8294881c20be"));

        return async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            Country targetCountry = await dbContext.Countries.SingleAsync(country => country.Id == targetCountryId);

            targetCountry.AddParticipantContestRole(fakeContestId);

            dbContext.Countries.Update(targetCountry);
            await dbContext.SaveChangesAsync();
        };
    }
}
