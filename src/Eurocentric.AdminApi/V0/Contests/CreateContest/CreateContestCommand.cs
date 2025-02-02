using ErrorOr;
using Eurocentric.Domain.Entities.Contests;
using MediatR;

namespace Eurocentric.AdminApi.V0.Contests.CreateContest;

public sealed record CreateContestCommand(int ContestYear, string HostCityName, VotingRules VotingRules)
    : IRequest<ErrorOr<Contest>>;
