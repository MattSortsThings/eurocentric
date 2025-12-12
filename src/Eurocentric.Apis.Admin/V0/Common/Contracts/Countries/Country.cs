namespace Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;

/// <summary>
///     Represents a country or pseudo-country.
/// </summary>
public sealed record Country
{
    /// <summary>
    ///     The country's ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     The country's ISO 3166 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     An unordered array of the country's contest roles.
    /// </summary>
    public ContestRole[] ContestRoles { get; init; } = [];

    public bool Equals(Country? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id.Equals(other.Id)
            && CountryCode == other.CountryCode
            && CountryName == other.CountryName
            && ContestRoles
                .OrderBy(role => role.ContestId)
                .SequenceEqual(other.ContestRoles.OrderBy(role => role.ContestId));
    }

    public override int GetHashCode() => HashCode.Combine(Id, CountryCode, CountryName, ContestRoles);
}
