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

    public required Competitor[] Competitors { get; init; }

    public required Voter[] Juries { get; init; }

    public required Voter[] Televotes { get; init; }

    public static Broadcast CreateExample() => new()
    {
        Id = ExampleIds.Broadcasts.Basel2025GrandFinal,
        BroadcastDate = DateOnly.ParseExact("2025-05-17", "yyyy-MM-dd"),
        ParentContestId = ExampleIds.Contests.Basel2025,
        ContestStage = ContestStage.GrandFinal,
        Competitors = [Competitor.CreateExample()],
        Juries = [Voter.CreateExample()],
        Televotes = [Voter.CreateExample()]
    };
}
