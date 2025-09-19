using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.WebApp.AcceptanceTests.Utils;

public sealed class SeededWebAppFixture : WebAppFixture
{
    private const string V0SchemaSeedingPath = "Eurocentric.WebApp.AcceptanceTests.Utils.Scripts.v0_schema_seeding.sql";

    [ClassDataSource<DbContainerFixture>(Shared = SharedType.PerClass)]
    public required DbContainerFixture DbContainerFixture { get; init; }

    private protected override async Task SeedDbAsync()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Database.ExecuteSqlRawAsync(ReadSqlFromEmbeddedResource(V0SchemaSeedingPath));
    }

    private protected override string GetDbConnectionString() => DbContainerFixture.GetConnectionString();
}
