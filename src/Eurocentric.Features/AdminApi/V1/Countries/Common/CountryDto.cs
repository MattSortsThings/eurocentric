using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Countries.Common;

public sealed record CountryDto(
    Guid Id,
    string CountryCode,
    string Name,
    ContestMemoDto[] ContestMemos) : IExampleProvider<CountryDto>
{
    public static CountryDto CreateExample() => new(Guid.Parse("13008a45-7363-4065-bbdb-59643f975903"),
        "GB",
        "United Kingdom",
        [ContestMemoDto.CreateExample()]);
}
