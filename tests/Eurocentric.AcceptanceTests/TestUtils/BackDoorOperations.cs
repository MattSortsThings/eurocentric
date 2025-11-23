using System.Reflection;
using Eurocentric.Components.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     Operations to be executed on the web application fixture's scoped service provider.
/// </summary>
public static class BackDoorOperations
{
    private static IEnumerable<string> GetSeedingScriptPathsInOrder()
    {
        yield return "Eurocentric.AcceptanceTests.TestUtils.Scripts.seed_dbo_1_of_5_50_countries.sql";
        yield return "Eurocentric.AcceptanceTests.TestUtils.Scripts.seed_dbo_2_of_5_2021_contest_not_queryable.sql";
        yield return "Eurocentric.AcceptanceTests.TestUtils.Scripts.seed_dbo_3_of_5_2022_contest_queryable.sql";
        yield return "Eurocentric.AcceptanceTests.TestUtils.Scripts.seed_dbo_4_of_5_2023_contest_queryable.sql";
        yield return "Eurocentric.AcceptanceTests.TestUtils.Scripts.seed_dbo_5_of_5_2024_contest_not_queryable.sql";
    }

    /// <summary>
    ///     Asynchronously pauses the test database container fixture.
    /// </summary>
    /// <param name="serviceProvider">The web application fixture's scoped service provider.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static async Task PauseDbAsync(IServiceProvider serviceProvider)
    {
        DbContainerFixtureSwitcher switcher = serviceProvider.GetRequiredService<DbContainerFixtureSwitcher>();

        await switcher.PauseAsync();
    }

    /// <summary>
    ///     Asynchronously ensures the test database container fixture is unpaused then deletes all records from the test
    ///     database.
    /// </summary>
    /// <param name="serviceProvider">The web application fixture's scoped service provider.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static async Task ResetDbAsync(IServiceProvider serviceProvider)
    {
        DbContainerFixtureSwitcher switcher = serviceProvider.GetRequiredService<DbContainerFixtureSwitcher>();
        await switcher.EnsureUnpausedAsync();

        await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Broadcasts.ExecuteDeleteAsync();
        await dbContext.Contests.ExecuteDeleteAsync();
        await dbContext.Countries.ExecuteDeleteAsync();
    }

    /// <summary>
    ///     Asynchronously populates the test database with seed data.
    /// </summary>
    /// <param name="serviceProvider">The web application fixture's scoped service provider.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static async Task SeedDbAsync(IServiceProvider serviceProvider)
    {
        await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

        foreach (string resourcePath in GetSeedingScriptPathsInOrder())
        {
            await dbContext.Database.ExecuteSqlRawAsync(ReadEmbeddedResource(resourcePath));
        }
    }

    private static string ReadEmbeddedResource(string resourcePath)
    {
        Assembly assembly = typeof(BackDoorOperations).Assembly;

        using Stream stream =
            assembly.GetManifestResourceStream(resourcePath)
            ?? throw new InvalidOperationException($"Embedded resource \"{resourcePath}\" not found.");

        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
