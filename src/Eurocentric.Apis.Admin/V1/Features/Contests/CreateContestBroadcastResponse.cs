using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

public sealed record CreateContestBroadcastResponse(Broadcast Broadcast)
    : ISchemaExampleProvider<CreateContestBroadcastResponse>
{
    public static CreateContestBroadcastResponse CreateExample()
    {
        Broadcast broadcast = new()
        {
            Id = V1ExampleValues.BroadcastId,
            ParentContestId = V1ExampleValues.ContestId,
            ContestStage = ContestStage.GrandFinal,
            BroadcastDate = V1ExampleValues.BroadcastDate,
            Completed = false,
            Competitors =
            [
                new Competitor
                {
                    RunningOrderSpot = 1,
                    FinishingPosition = 1,
                    CompetingCountryId = V1ExampleValues.CountryId1Of2,
                    JuryAwards = [],
                    TelevoteAwards = [],
                },
                new Competitor
                {
                    RunningOrderSpot = 3,
                    FinishingPosition = 2,
                    CompetingCountryId = V1ExampleValues.CountryId2Of2,
                    JuryAwards = [],
                    TelevoteAwards = [],
                },
            ],
            Juries =
            [
                new Jury { VotingCountryId = V1ExampleValues.CountryId1Of2, PointsAwarded = false },
                new Jury { VotingCountryId = V1ExampleValues.CountryId2Of2, PointsAwarded = false },
            ],
            Televotes =
            [
                new Televote { VotingCountryId = V1ExampleValues.CountryId1Of2, PointsAwarded = false },
                new Televote { VotingCountryId = V1ExampleValues.CountryId2Of2, PointsAwarded = false },
            ],
        };

        return new CreateContestBroadcastResponse(broadcast);
    }
}
