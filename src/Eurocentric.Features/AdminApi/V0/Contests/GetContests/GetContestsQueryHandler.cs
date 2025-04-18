using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Contests.Models;
using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.AdminApi.V0.Contests.GetContests;

internal sealed class GetContestsQueryHandler : IQueryHandler<GetContestsQuery, GetContestsResponse>
{
    public Task<ErrorOr<GetContestsResponse>> OnHandle(GetContestsQuery query, CancellationToken cancellationToken)
    {
        Contest[] contests =
        [
            new(Guid.Parse("f20a7b79-f746-441d-b0cb-f447980455ba"),
                2022,
                "Turin",
                ContestFormat.Stockholm, false),

            new(Guid.Parse("9c9cc0a6-a9af-457a-a49f-a3e5d097cb9e"),
                2023,
                "Liverpool",
                ContestFormat.Liverpool, false)
        ];

        return Task.FromResult(ErrorOrFactory.From(new GetContestsResponse(contests)));
    }
}
