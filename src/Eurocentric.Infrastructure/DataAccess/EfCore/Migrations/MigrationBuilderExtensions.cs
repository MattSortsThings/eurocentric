using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations;

internal static class MigrationBuilderExtensions
{
    internal static void ExecuteSqlFromEmbeddedResource(this MigrationBuilder migrationBuilder, string resourcePath)
    {
        string sql = ReadEmbeddedResource(resourcePath);
        migrationBuilder.Sql(sql);
    }

    private static string ReadEmbeddedResource(string resourcePath)
    {
        Assembly assembly = typeof(MigrationBuilderExtensions).Assembly;

        using Stream stream = assembly.GetManifestResourceStream(resourcePath)
                              ?? throw new InvalidOperationException($"Embedded resource '{resourcePath}' not found.");

        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
