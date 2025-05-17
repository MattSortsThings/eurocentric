using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Country(
    Guid Id,
    string CountryCode,
    string Name,
    ContestMemo[] ContestMemos) : IExampleProvider<Country>
{
    public static Country CreateExample() => new(Guid.Parse("13008a45-7363-4065-bbdb-59643f975903"),
        "GB",
        "United Kingdom",
        [ContestMemo.CreateExample()]);
}
