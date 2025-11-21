using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Contests;

/// <summary>
///     Represents a global televote in a contest.
/// </summary>
public sealed record GlobalTelevote : IDtoSchemaExampleProvider<GlobalTelevote>
{
    /// <summary>
    ///     The ID of the voting country.
    /// </summary>
    public Guid VotingCountryId { get; init; }

    public static GlobalTelevote CreateExample() => new() { VotingCountryId = V1ExampleIds.CountryC };

    public bool Equals(GlobalTelevote? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || VotingCountryId.Equals(other.VotingCountryId);
    }

    public override int GetHashCode() => VotingCountryId.GetHashCode();
}
