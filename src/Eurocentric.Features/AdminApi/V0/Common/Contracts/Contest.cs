using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V0.Common.Contracts;

public sealed record Contest : IExampleProvider<Contest>
{
    public required Guid Id { get; init; }

    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }

    public static Contest CreateExample() => new()
    {
        Id = ExampleIds.Contest, ContestYear = 2025, CityName = "Basel", ContestFormat = ContestFormat.Liverpool
    };
}
