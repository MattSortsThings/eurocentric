using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

public sealed class SeededWebAppFixture : WebAppFixture
{
    private const string ScriptsPathPrefix = "Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils.Scripts.";

    public override async Task InitializeAsync()
    {
        await StartDbContainerAndUseConnectionStringAsync();
        EnsureServerStarted();
        await MigrateDbAsync();
        await SeedDbAsync();
    }

    private async Task SeedDbAsync()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        string[] scriptFileNames =
        [
            "seeding_1_of_3_add_50_countries.sql",
            "seeding_2_of_3_add_2022_contest.sql",
            "seeding_3_of_3_add_2023_contest.sql"
        ];

        foreach (string fileName in scriptFileNames)
        {
            string sql = SqlScriptReader.ReadSqlFromEmbeddedResource(ScriptsPathPrefix + fileName);
            await dbContext.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
