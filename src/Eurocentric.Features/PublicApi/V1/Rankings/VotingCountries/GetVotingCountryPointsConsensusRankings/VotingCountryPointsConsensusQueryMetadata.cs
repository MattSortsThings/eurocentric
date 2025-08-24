using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsConsensusRankings;

public sealed record VotingCountryPointsConsensusQueryMetadata : IExampleProvider<VotingCountryPointsConsensusQueryMetadata>
{
    public required QueryableContestStage ContestStage { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public string? CompetingCountryCode { get; init; }


    public static VotingCountryPointsConsensusQueryMetadata CreateExample() => new()
    {
        ContestStage = QueryParamDefaults.ContestStage, MinYear = null, MaxYear = null, CompetingCountryCode = null
    };
}
