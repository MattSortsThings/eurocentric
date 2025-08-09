using System.Text.RegularExpressions;

namespace Eurocentric.Features.AcceptanceTests.Utils;

public static partial class MarkdownParser
{
    public static IEnumerable<T> ParseTable<T>(string? markdownTable, Func<Dictionary<string, string>, T> rowMapper)
    {
        if (string.IsNullOrEmpty(markdownTable))
        {
            return [];
        }

        string[] lines = NewLineRegex().Split(markdownTable)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Trim())
            .ToArray();

        string[] headers = lines[0]
            .Trim('|')
            .Split('|')
            .Select(cell => cell.Trim())
            .ToArray();

        return lines.Skip(2)
            .Select(line =>
            {
                string[] cells = line.Trim('|')
                    .Split('|')
                    .Select(cell => cell.Trim())
                    .ToArray();

                return headers.Zip(cells,
                        (header, cell) => new { key = header, value = cell })
                    .ToDictionary(kvp => kvp.key, kvp => kvp.value);
            }).Select(rowMapper);
    }

    [GeneratedRegex(@"\r\n|\n|\r")]
    private static partial Regex NewLineRegex();
}
