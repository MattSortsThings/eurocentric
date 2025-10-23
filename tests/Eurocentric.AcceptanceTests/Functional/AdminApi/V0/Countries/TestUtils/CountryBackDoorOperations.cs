using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.V0.Aggregates.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V0.Countries.TestUtils;

public static class CountryBackDoorOperations
{
    public static Func<IServiceProvider, Task> AddFakeContestRoleToCountry(Guid countryId)
    {
        Guid targetCountryId = countryId;

        return async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            Country targetCountry = await dbContext
                .V0Countries.Include(country => country.ContestRoles)
                .SingleAsync(country => country.Id == targetCountryId);

            targetCountry.ContestRoles.Add(
                new ContestRole
                {
                    ContestId = Guid.Parse("3d764835-1da8-4d52-8a93-8294881c20be"),
                    ContestRoleType = ContestRoleType.Participant,
                }
            );

            dbContext.V0Countries.Update(targetCountry);
            await dbContext.SaveChangesAsync();
        };
    }
}
