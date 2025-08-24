using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsInRangeRankings;

public sealed record VotingCountryPointsInRangeQueryMetadata : IExampleProvider<VotingCountryPointsInRangeQueryMetadata>
{
    public required QueryableContestStage ContestStage { get; init; }

    public required int MinPoints { get; init; }

    public required int MaxPoints { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public required string CompetingCountryCode { get; init; }

    public required QueryableVotingMethod VotingMethod { get; init; }

    public static VotingCountryPointsInRangeQueryMetadata CreateExample() => new()
    {
        ContestStage = QueryParamDefaults.ContestStage,
        MinPoints = ExampleValues.MinPoints,
        MaxPoints = ExampleValues.MaxPoints,
        MinYear = null,
        MaxYear = null,
        CompetingCountryCode = ExampleValues.CompetingCountryCode,
        VotingMethod = QueryParamDefaults.VotingMethod
    };
}
