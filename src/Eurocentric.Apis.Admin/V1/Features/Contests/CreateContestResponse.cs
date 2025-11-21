using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

public sealed record CreateContestResponse(Contest Contest) : IDtoSchemaExampleProvider<CreateContestResponse>
{
    public static CreateContestResponse CreateExample() => new(Contest.CreateExample() with { ChildBroadcasts = [] });
}
