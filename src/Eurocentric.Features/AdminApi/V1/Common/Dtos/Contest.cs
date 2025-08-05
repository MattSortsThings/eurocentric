using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Contest : IExampleProvider<Contest>
{
    public required Guid Id { get; init; }

    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }

    public required bool Completed { get; init; }

    public required ChildBroadcast[] ChildBroadcasts { get; init; }

    public required Participant[] Participants { get; init; }

    public static Contest CreateExample() => new()
    {
        Id = ExampleValues.ContestId,
        ContestYear = 2025,
        CityName = "Basel",
        ContestFormat = ContestFormat.Liverpool,
        Completed = false,
        ChildBroadcasts = [ChildBroadcast.CreateExample()],
        Participants = [Participant.CreateExample()]
    };
}
