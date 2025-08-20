using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsInRangeRankings;

public sealed record CompetitorPointsInRangeFilteringMetadata : IExampleProvider<CompetitorPointsInRangeFilteringMetadata>
{
    public required QueryableContestStage ContestStage { get; init; }

    public required int MinPoints { get; init; }

    public required int MaxPoints { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public required QueryableVotingMethod VotingMethod { get; init; }

    public static CompetitorPointsInRangeFilteringMetadata CreateExample() => new()
    {
        ContestStage = QueryParamDefaults.ContestStage,
        MinPoints = ExampleValues.MinPoints,
        MaxPoints = ExampleValues.MaxPoints,
        MinYear = null,
        MaxYear = null,
        VotingMethod = QueryParamDefaults.VotingMethod
    };
}
