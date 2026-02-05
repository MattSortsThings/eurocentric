using System.Reflection;
using System.Text.RegularExpressions;
using TUnit.Core.Interfaces;

namespace Eurocentric.Tests.Acceptance.Utils.Fixtures;

/// <summary>
///     Provides SQL batches to be executed in a test database.
/// </summary>
public sealed partial class SqlBatchProvider : IAsyncInitializer
{
    private const string ScriptsDirectoryPath = "Eurocentric.Tests.Acceptance.Utils.Scripts";
    private static readonly string[] MigrationFileNames = ["migrate_test_db.sql"];
    private static readonly string[] AugmentationFileNames = ["create_placeholder_usp_erase_all_data.sql"];
    private readonly List<string> _augmentationSqlBatches = [];

    private readonly List<string> _migrationSqlBatches = [];

    /// <summary>
    ///     Reads all the SQL batches from scripts and stores them in memory for the lifetime of the instance.
    /// </summary>
    public async Task InitializeAsync()
    {
        await ReadMigrationSqlBatchesAsync();
        await ReadAugmentationSqlBatchesAsync();
    }

    private async Task ReadMigrationSqlBatchesAsync()
    {
        string[][] sqlBatchesArray = await Task.WhenAll(MigrationFileNames.Select(ReadSqlBatchesFromScriptAsync));

        _migrationSqlBatches.AddRange(sqlBatchesArray.SelectMany(batches => batches));
    }

    private async Task ReadAugmentationSqlBatchesAsync()
    {
        string[][] sqlBatchesArray = await Task.WhenAll(AugmentationFileNames.Select(ReadSqlBatchesFromScriptAsync));

        _augmentationSqlBatches.AddRange(sqlBatchesArray.SelectMany(batches => batches));
    }

    /// <summary>
    ///     Enumerates the SQL batches to apply all source code migrations to a newly-created test database.
    /// </summary>
    /// <returns>An ordered sequence of SQL batches.</returns>
    public IEnumerable<string> GetMigrationSqlBatches() => _migrationSqlBatches.AsEnumerable();

    /// <summary>
    ///     Enumerates the SQL batches to apply all test augmentations to a migrated test database.
    /// </summary>
    /// <returns>An ordered sequence of SQL batches.</returns>
    public IEnumerable<string> GetAugmentationSqlBatches() => _augmentationSqlBatches.AsEnumerable();

    private static async Task<string[]> ReadSqlBatchesFromScriptAsync(string fileName)
    {
        Assembly assembly = typeof(DbContainer).Assembly;
        string resourcePath = ScriptsDirectoryPath + "." + fileName;

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
