using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Placeholders.Aggregates;

public sealed class PointsAward
{
    public required Guid VotingCountryId { get; init; }

    public required VotingMethod VotingMethod { get; init; }

    public required int PointsValue { get; init; }
}
