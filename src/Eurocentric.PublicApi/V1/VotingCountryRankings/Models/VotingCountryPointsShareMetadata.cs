using Eurocentric.PublicApi.V1.Models;

namespace Eurocentric.PublicApi.V1.VotingCountryRankings.Models;

public sealed record VotingCountryPointsShareMetadata(
    string TargetCountryCode,
    VotingMethod VotingMethod,
    int PageIndex,
    int PageSize);
