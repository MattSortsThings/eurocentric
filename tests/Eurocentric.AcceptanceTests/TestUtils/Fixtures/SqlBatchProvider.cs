using System.Reflection;
using System.Text.RegularExpressions;
using TUnit.Core.Interfaces;

namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

public sealed partial class SqlBatchProvider : IAsyncInitializer
{
    private const string ResourcePathPrefix = "Eurocentric.AcceptanceTests.TestUtils.Scripts.";
    private readonly List<string> _testDbMigrationSqlBatches = [];
    private readonly List<string> _testDbSprocCreationSqlBatches = [];

    public async Task InitializeAsync()
    {
        _testDbMigrationSqlBatches.AddRange(await ReadMigrateTestDbScriptAsync());
        _testDbSprocCreationSqlBatches.AddRange(await ReadCreateTestTbSprocScriptsAsync());
    }

    public IEnumerable<string> GetTestDbMigrationSqlBatches() => _testDbMigrationSqlBatches.AsEnumerable();

    public IEnumerable<string> GetTestDbSprocCreationSqlBatches() => _testDbSprocCreationSqlBatches.AsEnumerable();

    private static async Task<string[]> ReadMigrateTestDbScriptAsync() =>
        await ReadSqlBatchesFromScriptAsync("migrate_test_db.sql");

    private static async Task<string[]> ReadCreateTestTbSprocScriptsAsync()
    {
        string[][] batchArray = await Task.WhenAll(
            ReadSqlBatchesFromScriptAsync("create_placeholder_usp_erase_all_data.sql")
        );

        return batchArray.SelectMany(batch => batch).ToArray();
    }

    private static async Task<string[]> ReadSqlBatchesFromScriptAsync(string resourceFileName)
    {
        Assembly assembly = typeof(DbContainer).Assembly;

        string resourcePath = ResourcePathPrefix + resourceFileName;

        await using Stream? stream = assembly.GetManifestResourceStream(resourcePath);

        if (stream is null)
        {
            throw new ArgumentException($"Resource not found: '{resourcePath}'.");
        }

        using StreamReader reader = new(stream);

        string contents = await reader.ReadToEndAsync();

        return SplitOnGoAndFilterBatches(contents);
    }

    private static string[] SplitOnGoAndFilterBatches(string script) =>
        MultiLineGoRegex()
            .Split(script)
            .Select(sql => sql.Trim())
            .Where(sql => !string.IsNullOrWhiteSpace(sql))
            .ToArray();

    [GeneratedRegex(@"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase)]
    private static partial Regex MultiLineGoRegex();
}
