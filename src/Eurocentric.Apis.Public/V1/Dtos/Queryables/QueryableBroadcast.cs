using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.Queryables;

/// <summary>
///     A queryable broadcast.
/// </summary>
/// <remarks>A broadcast is queryable if its parent contest is queryable.</remarks>
public sealed record QueryableBroadcast : IDtoSchemaExampleProvider<QueryableBroadcast>
{
    /// <summary>
    ///     The date on which the broadcast is televised.
    /// </summary>
    public DateOnly BroadcastDate { get; init; }

    /// <summary>
    ///     The year in which the contest is held.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     The UK English name of the city in which the contest is held.
    /// </summary>
    public string CityName { get; init; } = string.Empty;

    /// <summary>
    ///     The broadcast's stage in its parent contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     The number of competitors in the broadcast.
    /// </summary>
    public int Competitors { get; init; }

    /// <summary>
    ///     The number of juries in the broadcast.
    /// </summary>
    public int Juries { get; init; }

    /// <summary>
    ///     The number of televotes in the broadcast.
    /// </summary>
    public int Televotes { get; init; }

    public static QueryableBroadcast CreateExample()
    {
        return new QueryableBroadcast
        {
            BroadcastDate = DateOnly.ParseExact("2025-05-17", "yyyy-MM-dd"),
            ContestYear = 2025,
            CityName = "Basel",
            ContestStage = ContestStage.GrandFinal,
            Competitors = 26,
            Juries = 36,
            Televotes = 37,
        };
    }

    public bool Equals(QueryableBroadcast? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return BroadcastDate.Equals(other.BroadcastDate)
            && ContestYear == other.ContestYear
            && CityName == other.CityName
            && ContestStage == other.ContestStage
            && Competitors == other.Competitors
            && Juries == other.Juries
            && Televotes == other.Televotes;
    }

    public override int GetHashCode() =>
        HashCode.Combine(BroadcastDate, ContestYear, CityName, (int)ContestStage, Competitors, Juries, Televotes);
}
