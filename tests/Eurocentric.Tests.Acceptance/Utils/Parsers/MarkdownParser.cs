using System.Text.RegularExpressions;

namespace Eurocentric.Tests.Acceptance.Utils.Parsers;

public static partial class MarkdownParser
{
    /// <summary>
    ///     Parses an array of objects from the sequential rows in a Markdown table.
    /// </summary>
    /// <param name="markdownTable">The table to be parsed.</param>
    /// <param name="rowMapper">
    ///     A function that constructs a new instance of type <typeparamref name="TItem" /> from a
    ///     dictionary parsed from the table, in which empty cells are parsed as empty strings.
    /// </param>
    /// <typeparam name="TItem">The return value item type.</typeparam>
    /// <returns>
    ///     An array of objects of type <typeparamref name="TItem" />.
    /// </returns>
    public static TItem[] ParseTable<TItem>(string? markdownTable, Func<Dictionary<string, string>, TItem> rowMapper)
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
            .Select(rowMapper)
            .ToArray();
    }

    [GeneratedRegex(@"\r\n|\n|\r")]
    private static partial Regex NewLineRegex();
}
