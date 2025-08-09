using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a televote in a broadcast.
/// </summary>
public sealed class Televote : Voter
{
    [UsedImplicitly(Reason = "EF Core")]
    private Televote()
    {
    }

    public Televote(CountryId votingCountryId) : base(votingCountryId)
    {
    }

    private protected override void GivePointsAward(Competitor competitor, PointsValue pointsValue) =>
        competitor.ReceiveAward(new TelevoteAward(VotingCountryId, pointsValue));
}
