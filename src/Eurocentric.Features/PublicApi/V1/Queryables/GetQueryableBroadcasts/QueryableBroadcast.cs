using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableBroadcasts;

public sealed record QueryableBroadcast : IExampleProvider<QueryableBroadcast>
{
    public required int ContestYear { get; init; }

    public required ContestStage ContestStage { get; init; }

    public required DateOnly BroadcastDate { get; init; }

    public required int Competitors { get; init; }

    public required int Juries { get; init; }

    public required int Televotes { get; init; }

    public static QueryableBroadcast CreateExample() => new()
    {
        ContestYear = 2025,
        ContestStage = ContestStage.GrandFinal,
        BroadcastDate = new DateOnly(2025, 5, 17),
        Competitors = 26,
        Juries = 37,
        Televotes = 38
    };
}
