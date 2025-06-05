using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Country : IExampleProvider<Country>
{
    public required Guid Id { get; init; }

    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public required ContestMemo[] ContestMemos { get; init; }

    public static Country CreateExample() => new()
    {
        Id = ExampleIds.Countries.Austria,
        CountryCode = "AT",
        CountryName = "Austria",
        ContestMemos =
        [
            ContestMemo.CreateExample()
        ]
    };
}
