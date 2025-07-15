namespace Eurocentric.Features.AdminApi.V1.Common.Contracts;

public sealed record PointsAward
{
    /// <summary>
    ///     The voting country ID.
    /// </summary>
    public required Guid VotingCountryId { get; init; }

    /// <summary>
    ///     The numeric points value of the award.
    /// </summary>
    public required int PointsValue { get; init; }
}
