using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Broadcast : IExampleProvider<Broadcast>
{
    public required Guid Id { get; init; }

    public required DateOnly BroadcastDate { get; init; }

    public required Guid ParentContestId { get; init; }

    public required ContestStage ContestStage { get; init; }

    public required bool Completed { get; init; }

    public required Competitor[] Competitors { get; init; }

    public required Voter[] Juries { get; init; }

    public required Voter[] Televotes { get; init; }

    public static Broadcast CreateExample()
    {
        Voter[] voters = [Voter.CreateExample()];

        return new Broadcast
        {
            Id = ExampleValues.BroadcastId,
            BroadcastDate = DateOnly.ParseExact("2025-05-17", "yyyy-MM-dd"),
            ParentContestId = ExampleValues.ContestId,
            ContestStage = ContestStage.GrandFinal,
            Completed = false,
            Competitors = [Competitor.CreateExample()],
            Juries = voters,
            Televotes = voters
        };
    }
}
