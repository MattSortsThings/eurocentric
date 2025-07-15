namespace Eurocentric.Features.AdminApi.V1.Common.Contracts;

public sealed record Voter
{
    /// <summary>
    ///     The voting country ID.
    /// </summary>
    public required Guid VotingCountryId { get; init; }

    /// <summary>
    ///     Indicates whether the voter has awarded its points.
    /// </summary>
    public required bool PointsAwarded { get; init; }
}
