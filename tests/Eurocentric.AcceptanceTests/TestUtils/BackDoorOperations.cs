using System.Reflection;
using Eurocentric.Components.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AcceptanceTests.TestUtils;

public static class BackDoorOperations
{
    public static async Task EraseAllDataAsync(IServiceProvider serviceProvider)
    {
        await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

        await dbContext.V0Broadcasts.ExecuteDeleteAsync();
        await dbContext.V0Contests.ExecuteDeleteAsync();
        await dbContext.V0Countries.ExecuteDeleteAsync();
    }

    public static Func<IServiceProvider, Task> ExecuteSqlFromScriptAsync(string scriptPath)
    {
        string sql = ReadEmbeddedResource(scriptPath);

        return async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Database.ExecuteSqlRawAsync(sql);
        };
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
