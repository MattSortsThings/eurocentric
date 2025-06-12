using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Broadcasts;

/// <summary>
///     Represents a competitor in a broadcast.
/// </summary>
public sealed class Competitor : Entity
{
    private readonly List<JuryAward> _juryAwards = [];
    private readonly List<TelevoteAward> _televoteAwards = [];

    private Competitor()
    {
    }

    public Competitor(CountryId competingCountryId, int runningOrderPosition)
    {
        CompetingCountryId = competingCountryId;
        FinishingPosition = runningOrderPosition;
        RunningOrderPosition = runningOrderPosition;
    }

    /// <summary>
    ///     Gets the ID of the country aggregate that the competitor represents.
    /// </summary>
    public CountryId CompetingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets the competitor's finishing position in its broadcast.
    /// </summary>
    public int FinishingPosition { get; internal set; } = 1;

    /// <summary>
    ///     Gets the competitor's running order position in its broadcast.
    /// </summary>
    public int RunningOrderPosition { get; } = 1;

    /// <summary>
    ///     Gets a list of all the jury awards received by the competitor, ordered by descending points value then by ascending
    ///     voting country ID value.
    /// </summary>
    public IReadOnlyList<JuryAward> JuryAwards =>
        _juryAwards.OrderByDescending(award => award.PointsValue)
            .ThenBy(award => award.VotingCountryId.Value)
            .ToArray();

    /// <summary>
    ///     Gets a list of all the televote awards received by the competitor, ordered by descending points value then by
    ///     ascending voting country ID value.
    /// </summary>
    public IReadOnlyList<TelevoteAward> TelevoteAwards =>
        _televoteAwards.OrderByDescending(award => award.PointsValue)
            .ThenBy(award => award.VotingCountryId.Value)
            .ToArray();

    internal void ReceiveAward(JuryAward juryAward) => _juryAwards.Add(juryAward);

    internal void ReceiveAward(TelevoteAward televoteAward) => _televoteAwards.Add(televoteAward);

    internal static IComparer<Competitor> GetBroadcastCompetitorComparer()
    {
        OverallTotalPointsComparer a = new();
        TotalTelevotePointsComparer b = new();
        NonZeroTelevoteAwardsComparer c = new();
        TelevoteCountBackComparer d = new();
        RunningOrderPositionComparer e = new();

        a.Next = b;
        b.Next = c;
        c.Next = d;
        d.Next = e;

        return a;
    }

    private int GetOverallTotalPoints() => GetTotalJuryPoints() + GetTotalTelevotePoints();

    private int GetTotalJuryPoints() => _juryAwards.Sum(award => (int)award.PointsValue);

    private int GetTotalTelevotePoints() => _televoteAwards.Sum(award => (int)award.PointsValue);

    private int GetNonZeroTelevoteAwardCount() => _televoteAwards.Count(award => award.PointsValue != PointsValue.Zero);

    private abstract class ChainedComparer : IComparer<Competitor>
    {
        public ChainedComparer? Next { get; set; }

        public int Compare(Competitor? x, Competitor? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (y is null)
            {
                return 1;
            }

            if (x is null)
            {
                return -1;
            }

            return CompareByMembers(x, y) is var memberComparison && memberComparison != 0
                ? memberComparison
                : Next?.Compare(x, y) ?? 0;
        }

        private protected abstract int CompareByMembers(Competitor x, Competitor y);
    }

    private sealed class OverallTotalPointsComparer : ChainedComparer
    {
        private protected override int CompareByMembers(Competitor x, Competitor y)
        {
            int c = x.GetOverallTotalPoints().CompareTo(y.GetOverallTotalPoints());

            return -c;
        }
    }

    private sealed class TotalTelevotePointsComparer : ChainedComparer
    {
        private protected override int CompareByMembers(Competitor x, Competitor y)
        {
            int c = x.GetTotalTelevotePoints().CompareTo(y.GetTotalTelevotePoints());

            return -c;
        }
    }

    private sealed class NonZeroTelevoteAwardsComparer : ChainedComparer
    {
        private protected override int CompareByMembers(Competitor x, Competitor y)
        {
            int c = x.GetNonZeroTelevoteAwardCount().CompareTo(y.GetNonZeroTelevoteAwardCount());

            return -c;
        }
    }

    private sealed class TelevoteCountBackComparer : ChainedComparer
    {
        private protected override int CompareByMembers(Competitor x, Competitor y) =>
            x._televoteAwards.OrderByDescending(award => award.PointsValue)
                .Zip(y._televoteAwards.OrderByDescending(award => award.PointsValue),
                    (xAward, yAward) =>
                    {
                        int comparison = xAward.PointsValue.CompareTo(yAward.PointsValue);

                        return -comparison;
                    })
                .FirstOrDefault(comparison => comparison != 0, 0);
    }

    private sealed class RunningOrderPositionComparer : ChainedComparer
    {
        private protected override int CompareByMembers(Competitor x, Competitor y) =>
            x.RunningOrderPosition.CompareTo(y.RunningOrderPosition);
    }
}
