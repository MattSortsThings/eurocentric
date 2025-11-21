using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Features.Broadcasts;

public sealed record AwardBroadcastJuryPointsRequest : IDtoSchemaExampleProvider<AwardBroadcastJuryPointsRequest>
{
    /// <summary>
    ///     The voting country ID of the jury to award points.
    /// </summary>
    public required Guid VotingCountryId { get; init; }

    /// <summary>
    ///     The competing country IDs in rank order from first to last.
    /// </summary>
    public required Guid[] RankedCompetingCountryIds { get; init; }

    public static AwardBroadcastJuryPointsRequest CreateExample() =>
        new() { VotingCountryId = V1ExampleIds.CountryB, RankedCompetingCountryIds = [V1ExampleIds.CountryA] };

    public bool Equals(AwardBroadcastJuryPointsRequest? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return VotingCountryId.Equals(other.VotingCountryId)
            && RankedCompetingCountryIds.SequenceEqual(other.RankedCompetingCountryIds);
    }

    public override int GetHashCode() => HashCode.Combine(VotingCountryId, RankedCompetingCountryIds);
}
