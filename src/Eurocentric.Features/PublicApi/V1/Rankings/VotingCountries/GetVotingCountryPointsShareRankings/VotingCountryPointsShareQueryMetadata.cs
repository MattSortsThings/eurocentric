using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsShareRankings;

public sealed record VotingCountryPointsShareQueryMetadata : IExampleProvider<VotingCountryPointsShareQueryMetadata>
{
    public required QueryableContestStage ContestStage { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public required string CompetingCountryCode { get; init; }

    public required QueryableVotingMethod VotingMethod { get; init; }

    public static VotingCountryPointsShareQueryMetadata CreateExample() => new()
    {
        ContestStage = QueryParamDefaults.ContestStage,
        MinYear = null,
        MaxYear = null,
        CompetingCountryCode = ExampleValues.CompetingCountryCode,
        VotingMethod = QueryParamDefaults.VotingMethod
    };
}
