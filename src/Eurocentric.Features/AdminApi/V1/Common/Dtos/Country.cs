using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Country(
    Guid Id,
    string CountryCode,
    string Name,
    ContestMemo[] ContestMemos) : IExampleProvider<Country>
{
    public static Country CreateExample() => new(ExampleValues.CountryId,
        "AT",
        "Austria",
        [ContestMemo.CreateExample()]);
}
