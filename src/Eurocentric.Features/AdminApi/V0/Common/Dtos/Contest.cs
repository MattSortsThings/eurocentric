using Eurocentric.Features.AdminApi.V0.Common.Enums;

namespace Eurocentric.Features.AdminApi.V0.Common.Dtos;

public sealed record Contest
{
    public Guid Id { get; init; }

    public int ContestYear { get; init; }

    public string CityName { get; init; } = string.Empty;

    public ContestFormat ContestFormat { get; init; }

    public bool Completed { get; init; }

    public ChildBroadcast[] ChildBroadcasts { get; init; } = [];

    public Participant[] Participants { get; init; } = [];
}
