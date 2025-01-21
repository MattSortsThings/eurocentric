using Eurocentric.Domain.Queries.Common;

namespace Eurocentric.Domain.Queries.VotingCountryRankings;

public sealed record VotingCountryPointsShareMetadata
{
    public required string TargetCountryCode { get; init; }

    public required VotingMethod VotingMethod { get; init; }

    public required int PageIndex { get; init; }

    public required int PageSize { get; init; }
}
