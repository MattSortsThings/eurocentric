using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsInRangeRankings;

public sealed record CompetingCountryPointsInRangeFilteringMetadata :
    IExampleProvider<CompetingCountryPointsInRangeFilteringMetadata>
{
    public required QueryableContestStage ContestStage { get; init; }

    public required int MinPoints { get; init; }

    public required int MaxPoints { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public string? VotingCountryCode { get; init; }

    public required QueryableVotingMethod VotingMethod { get; init; }

    public static CompetingCountryPointsInRangeFilteringMetadata CreateExample() => new()
    {
        ContestStage = QueryParamDefaults.ContestStage,
        MinPoints = ExampleValues.MinPoints,
        MaxPoints = ExampleValues.MaxPoints,
        MinYear = null,
        MaxYear = null,
        VotingCountryCode = null,
        VotingMethod = QueryParamDefaults.VotingMethod
    };
}
