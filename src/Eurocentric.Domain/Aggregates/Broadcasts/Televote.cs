using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a televote in a broadcast.
/// </summary>
public sealed class Televote : Voter
{
    [UsedImplicitly(Reason = "EF Core")]
    private Televote() { }

    internal Televote(CountryId votingCountryId)
        : base(votingCountryId) { }

    private protected override void GivePointsAward(Competitor competitor, PointsValue pointsValue) =>
        competitor.ReceivePointsAward(new TelevoteAward(VotingCountryId, pointsValue));
}
