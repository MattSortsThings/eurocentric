using Eurocentric.PublicApi.V0.Models;

namespace Eurocentric.PublicApi.V0.VotingCountryRankings.Models;

public sealed record VotingCountryPointsShareMetadata(
    string TargetCountryCode,
    VotingMethod VotingMethod,
    int PageIndex,
    int PageSize);
