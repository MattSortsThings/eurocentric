using Eurocentric.Domain.ValueObjects;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.Helpers.Countries;

public static class BackDoorOperations
{
    public static Func<IServiceProvider, Task> DeleteCountryAsync(CountryId countryId)
    {
        CountryId idToDelete = countryId;

        return async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Countries.Where(country => country.Id == idToDelete).ExecuteDeleteAsync();
        };
    }
}
