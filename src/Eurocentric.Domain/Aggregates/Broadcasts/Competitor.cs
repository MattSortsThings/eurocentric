using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a competitor in a broadcast.
/// </summary>
public sealed class Competitor : Entity
{
    private readonly List<JuryAward> _juryAwards = [];
    private readonly List<TelevoteAward> _televoteAwards = [];

    [UsedImplicitly(Reason = "EF Core")]
    private Competitor() { }

    internal Competitor(
        CountryId competingCountryId,
        RunningOrderSpot runningOrderSpot,
        FinishingPosition finishingPosition
    )
    {
        CompetingCountryId = competingCountryId;
        RunningOrderSpot = runningOrderSpot;
        FinishingPosition = finishingPosition;
    }

    /// <summary>
    ///     Gets the ID of the competing country.
    /// </summary>
    public CountryId CompetingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets the competitor's running order spot in the broadcast.
    /// </summary>
    public RunningOrderSpot RunningOrderSpot { get; } = null!;

    /// <summary>
    ///     Gets the competitor's finishing position in the broadcast.
    /// </summary>
    public FinishingPosition FinishingPosition { get; internal set; } = null!;

    /// <summary>
    ///     Gets a list of all the jury awards the competitor received.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a copy of the country's jury award list.</remarks>
    public IReadOnlyList<JuryAward> JuryAwards => _juryAwards.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets a list of all the televote awards the competitor received.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a copy of the country's televote award list.</remarks>
    public IReadOnlyList<TelevoteAward> TelevoteAwards => _televoteAwards.ToArray().AsReadOnly();

    /// <summary>
    ///     Creates and returns an object that compares pairs of competitors using the Eurovision broadcast finishing position
    ///     ordering rules.
    /// </summary>
    internal static IComparer<Competitor> BroadcastCompetitorComparer
    {
        get
        {
            OverallTotalPointsComparer a = new();
            TotalTelevotePointsComparer b = new();
            NonZeroTelevoteAwardsComparer c = new();
            TelevoteCountBackComparer d = new();
            RunningOrderSpotComparer e = new();

            a.Next = b;
            b.Next = c;
            c.Next = d;
            d.Next = e;

            return a;
        }
    }

    internal void ReceivePointsAward(JuryAward juryAward) => _juryAwards.Add(juryAward);

    internal void ReceivePointsAward(TelevoteAward televoteAward) => _televoteAwards.Add(televoteAward);

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
            x
                ._televoteAwards.OrderByDescending(award => award.PointsValue)
                .Zip(
                    y._televoteAwards.OrderByDescending(award => award.PointsValue),
                    (xAward, yAward) =>
                    {
                        int comparison = xAward.PointsValue.CompareTo(yAward.PointsValue);

                        return -comparison;
                    }
                )
                .FirstOrDefault(comparison => comparison != 0, 0);
    }

    private sealed class RunningOrderSpotComparer : ChainedComparer
    {
        private protected override int CompareByMembers(Competitor x, Competitor y) =>
            x.RunningOrderSpot.CompareTo(y.RunningOrderSpot);
    }
}
