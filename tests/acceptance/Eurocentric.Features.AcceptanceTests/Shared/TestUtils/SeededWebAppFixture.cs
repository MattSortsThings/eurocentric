using Eurocentric.Domain.V0.Entities;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.Shared.TestUtils;

public sealed class SeededWebAppFixture : WebAppFixture
{
    [ClassDataSource<DbContainerFixture>(Shared = SharedType.PerClass)]
    public override required DbContainerFixture DbContainerFixture { get; init; }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await SeedDbWithExistingCountryAsync();
    }

    private async Task SeedDbWithExistingCountryAsync()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        Country existingCountry = new()
        {
            Id = TestConstants.ExistingCountry.Id,
            CountryName = TestConstants.ExistingCountry.CountryName,
            CountryCode = TestConstants.ExistingCountry.CountryCode,
            ParticipatingContestIds = []
        };

        dbContext.V0Countries.Add(existingCountry);
        await dbContext.SaveChangesAsync();
    }
}
