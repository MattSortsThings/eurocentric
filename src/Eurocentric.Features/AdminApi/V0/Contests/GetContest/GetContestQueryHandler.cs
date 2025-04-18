using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Contests.Models;
using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.AdminApi.V0.Contests.GetContest;

internal sealed class GetContestQueryHandler : IQueryHandler<GetContestQuery, GetContestResponse>
{
    public Task<ErrorOr<GetContestResponse>> OnHandle(GetContestQuery query, CancellationToken cancellationToken)
    {
        Contest contest = new(query.ContestId, 2023, "Liverpool", ContestFormat.Liverpool, false);

        return Task.FromResult(ErrorOrFactory.From(new GetContestResponse(contest)));
    }
}
