namespace Eurocentric.Features.AdminApi.V0.Common.Contracts;

public sealed record Contest
{
    public required Guid Id { get; init; }

    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }
}
