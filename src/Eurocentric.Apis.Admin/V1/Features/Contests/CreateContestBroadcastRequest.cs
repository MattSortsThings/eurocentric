using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

public sealed record CreateContestBroadcastRequest : IDtoSchemaExampleProvider<CreateContestBroadcastRequest>
{
    /// <summary>
    ///     The date on which the broadcast is televised.
    /// </summary>
    public required DateOnly BroadcastDate { get; init; }

    /// <summary>
    ///     The broadcast's stage in its parent contest.
    /// </summary>
    public required ContestStage ContestStage { get; init; }

    /// <summary>
    ///     The IDs of the competing countries, in broadcast running order, with vacant running order spots represented by
    ///     <see langword="null" /> values.
    /// </summary>
    public required Guid?[] CompetingCountryIds { get; init; }

    public static CreateContestBroadcastRequest CreateExample() =>
        new()
        {
            ContestStage = ContestStage.GrandFinal,
            BroadcastDate = DateOnly.ParseExact("2025-05-17", "yyyy-MM-dd"),
            CompetingCountryIds = [V1ExampleIds.CountryA],
        };

    public bool Equals(CreateContestBroadcastRequest? other)
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
            && ContestStage == other.ContestStage
            && CompetingCountryIds.Equals(other.CompetingCountryIds);
    }

    public override int GetHashCode() => HashCode.Combine(BroadcastDate, (int)ContestStage, CompetingCountryIds);
}
