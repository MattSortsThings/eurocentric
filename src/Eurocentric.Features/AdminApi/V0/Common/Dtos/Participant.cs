namespace Eurocentric.Features.AdminApi.V0.Common.Dtos;

public sealed record Participant
{
    public required Guid ParticipatingCountryId { get; init; }

    public required int ParticipantGroup { get; init; }

    public required string? ActName { get; init; }

    public required string? SongTitle { get; init; }
}
