using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.AdminApi.V0.Contests.GetContest;

public sealed record GetContestQuery(Guid ContestId) : IQuery<GetContestResponse>;
