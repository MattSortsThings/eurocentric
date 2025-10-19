using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eurocentric.Components.DataAccess.EfCore.Migrations;

internal static class MigrationBuilderExtensions
{
    internal static void ExecuteSqlFromScript(this MigrationBuilder migrationBuilder, string scriptFileName)
    {
        string resourceName = "Eurocentric.Components.DataAccess.Scripts." + scriptFileName;
        string sql = ReadEmbeddedResource(resourceName);
        migrationBuilder.Sql(sql);
    }

    private static string ReadEmbeddedResource(string resourceName)
    {
        Assembly assembly = typeof(MigrationBuilderExtensions).Assembly;

        using Stream stream =
            assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");

        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
