using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Contests.CreateChildBroadcast;

public sealed record CreateChildBroadcastResponse(Broadcast Broadcast) : IExampleProvider<CreateChildBroadcastResponse>
{
    public static CreateChildBroadcastResponse CreateExample()
    {
        Voter[] voters = [Voter.CreateExample() with { PointsAwarded = false }];

        Broadcast broadcast = new()
        {
            Id = ExampleValues.BroadcastId,
            BroadcastDate = new DateOnly(2025, 5, 17),
            ParentContestId = ExampleValues.ContestId,
            ContestStage = ContestStage.GrandFinal,
            Completed = false,
            Competitors =
            [
                new Competitor
                {
                    RunningOrderPosition = 1,
                    FinishingPosition = 1,
                    CompetingCountryId = ExampleValues.CountryId1Of3,
                    JuryAwards = [],
                    TelevoteAwards = []
                }
            ],
            Televotes = voters,
            Juries = voters
        };

        return new CreateChildBroadcastResponse(broadcast);
    }
}
