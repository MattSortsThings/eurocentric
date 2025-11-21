using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

public sealed record CreateContestBroadcastResponse(Broadcast Broadcast)
    : IDtoSchemaExampleProvider<CreateContestBroadcastResponse>
{
    public static CreateContestBroadcastResponse CreateExample() =>
        new(
            Broadcast.CreateExample() with
            {
                Competitors = [Competitor.CreateExample() with { JuryAwards = [], TelevoteAwards = [] }],
            }
        );
}
