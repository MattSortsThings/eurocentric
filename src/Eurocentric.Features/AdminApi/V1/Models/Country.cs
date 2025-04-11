namespace Eurocentric.Features.AdminApi.V1.Models;

public sealed record Country
{
    public required Guid Id { get; init; }

    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public required Guid[] ContestIds { get; init; }
}
