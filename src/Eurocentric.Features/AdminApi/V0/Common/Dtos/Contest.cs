using Eurocentric.Features.AdminApi.V0.Common.Enums;

namespace Eurocentric.Features.AdminApi.V0.Common.Dtos;

public sealed record Contest
{
    public required Guid Id { get; init; }

    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }

    public required bool Completed { get; init; }

    public required ChildBroadcast[] ChildBroadcasts { get; init; }

    public required Participant[] Participants { get; init; }
}
