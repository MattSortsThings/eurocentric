using System.Reflection;

namespace Eurocentric.Features.AcceptanceTests.Utils;

public static class SqlScriptReader
{
    public static string ReadSqlFromEmbeddedResource(string resourcePath)
    {
        Assembly assembly = typeof(SqlScriptReader).Assembly;

        using Stream stream = assembly.GetManifestResourceStream(resourcePath)
                              ?? throw new InvalidOperationException($"Embedded resource '{resourcePath}' not found.");

        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
