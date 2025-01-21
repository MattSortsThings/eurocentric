using ErrorOr;
using Eurocentric.Domain.Queries.Common;
using Eurocentric.Domain.Queries.VotingCountryRankings;
using MediatR;

namespace Eurocentric.PublicApi.V1.VotingCountryRankings.GetVotingCountryPointsShareRankings;

public sealed record GetVotingCountryPointsShareRankingsQuery(
    string TargetCountryCode,
    VotingMethod? VotingMethod = null,
    int? PageIndex = null,
    int? PageSize = null) : IRequest<ErrorOr<VotingCountryPointsSharePage>>;
