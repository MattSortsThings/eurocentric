using System.Text.RegularExpressions;

namespace Eurocentric.AcceptanceTests.TestUtils;

public static partial class MarkdownParser
{
    /// <summary>
    ///     Parses a sequence of objects from the sequential rows in a Markdown table.
    /// </summary>
    /// <param name="markdownTable">The table to be parsed.</param>
    /// <param name="rowMapper">
    ///     A function that constructs a new instance of type <typeparamref name="TItem" /> from a
    ///     dictionary parsed from the table, in which empty cells are parsed as empty strings.
    /// </param>
    /// <typeparam name="TItem">The return value item type.</typeparam>
    /// <returns>
    ///     A sequence of objects of type <typeparamref name="TItem" />; or an empty sequence if the
    ///     <paramref name="markdownTable" /> parameter is <see langword="null" />.
    /// </returns>
    public static IEnumerable<TItem> ParseTable<TItem>(
        string? markdownTable,
        Func<Dictionary<string, string>, TItem> rowMapper
    )
    {
        if (string.IsNullOrEmpty(markdownTable))
        {
            return [];
        }

        string[] lines = NewLineRegex()
            .Split(markdownTable)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Trim())
            .ToArray();

        string[] headers = lines[0].Trim('|').Split('|').Select(cell => cell.Trim()).ToArray();

        return lines
            .Skip(2)
            .Select(line =>
            {
                IEnumerable<string> cells = line.Trim('|').Split('|').Select(cell => cell.Trim());

                return headers
                    .Zip(cells, (header, cell) => new { key = header, value = cell })
                    .ToDictionary(kvp => kvp.key, kvp => kvp.value);
            })
            .Select(rowMapper);
    }

    [GeneratedRegex(@"\r\n|\n|\r")]
    private static partial Regex NewLineRegex();
}
