using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Eurocentric.Features.AcceptanceTests.Utilities;

public static class StringExtensions
{
    public static TItem[] ParseItems<TItem>(this string pipeSeparatedTable)
    {
        CsvConfiguration csvConfig = new(CultureInfo.InvariantCulture)
        {
            TrimOptions = TrimOptions.Trim, HasHeaderRecord = true, NewLine = Environment.NewLine, Delimiter = "|"
        };

        using StringReader reader = new(pipeSeparatedTable);
        using CsvReader csv = new(reader, csvConfig);

        return csv.GetRecords<TItem>().ToArray();
    }
}
