using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableContests;

public sealed record QueryableContest : IExampleProvider<QueryableContest>
{
    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required int Competitors { get; init; }

    public required bool HasRestOfWorldTelevotes { get; init; }

    public static QueryableContest CreateExample() => new()
    {
        ContestYear = 2025, CityName = "Basel", Competitors = 37, HasRestOfWorldTelevotes = true
    };
}
