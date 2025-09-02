using System.Reflection;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public static class SqlScriptReader
{
    public static string ReadSqlFromEmbeddedResource(string fullResourcePath)
    {
        Assembly assembly = typeof(SqlScriptReader).Assembly;

        using Stream stream = assembly.GetManifestResourceStream(fullResourcePath)
                              ?? throw new InvalidOperationException($"Embedded resource '{fullResourcePath}' not found.");

        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
