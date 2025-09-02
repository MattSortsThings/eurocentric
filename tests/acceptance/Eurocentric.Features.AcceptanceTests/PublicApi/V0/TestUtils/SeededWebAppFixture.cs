using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;

public sealed class SeededWebAppFixture : WebAppFixture
{
    [ClassDataSource<DbContainerFixture>(Shared = SharedType.PerClass)]
    public override required DbContainerFixture DbContainerFixture { get; init; }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await SeedDbAsync();
    }

    private async Task SeedDbAsync()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        foreach (string path in GetSeedingScriptPaths())
        {
            string sql = SqlScriptReader.ReadSqlFromEmbeddedResource(path);
            await dbContext.Database.ExecuteSqlRawAsync(sql);
        }
    }

    private static IEnumerable<string> GetSeedingScriptPaths()
    {
        const string scriptsDirectory = "Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils.Scripts";

        yield return scriptsDirectory + ".seeding_1_of_3_add_50_countries.sql";
        yield return scriptsDirectory + ".seeding_2_of_3_add_2022_contest.sql";
        yield return scriptsDirectory + ".seeding_3_of_3_add_2023_contest.sql";
    }
}
