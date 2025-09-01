namespace Eurocentric.Features.AdminApi.V0.Common.Dtos;

public sealed record Country
{
    public required Guid Id { get; init; }

    public required string CountryCode { get; init; }

    public required string CityName { get; init; }

    public required Guid[] ParticipatingContestIds { get; init; }
}
