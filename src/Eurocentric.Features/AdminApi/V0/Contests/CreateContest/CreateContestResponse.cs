using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V0.Contests.CreateContest;

public sealed record CreateContestResponse(Contest Contest) : IExampleProvider<CreateContestResponse>
{
    public static CreateContestResponse CreateExample() => new(Contest.CreateExample() with { ChildBroadcasts = [] });
}
