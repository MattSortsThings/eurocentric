using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Represents a single country or pseudo-country.
/// </summary>
public sealed class Country : AggregateRoot<CountryId>
{
    private readonly List<ContestId> _participatingContestIds = [];

    [UsedImplicitly(Reason = "EF Core")]
    private Country()
    {
    }

    public Country(CountryId id, CountryCode countryCode, CountryName countryName) : base(id)
    {
        CountryCode = countryCode;
        CountryName = countryName;
    }

    /// <summary>
    ///     Gets the country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public CountryCode CountryCode { get; private init; } = null!;

    /// <summary>
    ///     Gets the country's short UK English name.
    /// </summary>
    public CountryName CountryName { get; private init; } = null!;

    /// <summary>
    ///     Gets the IDs of all the contests in which the country is a participant.
    /// </summary>
    /// <remarks>
    ///     This property creates and returns a new collection every time it is accessed. No assumptions should be made
    ///     about the ordering of items.
    /// </remarks>
    public IReadOnlyList<ContestId> ParticipatingContestIds => _participatingContestIds
        .ToArray()
        .AsReadOnly();

    public static Country CreateDummyCountry(Guid countryId)
    {
        CountryId id = CountryId.FromValue(countryId);
        CountryCode countryCode = CountryCode.FromValue("AT").Value;
        CountryName countryName = CountryName.FromValue("Austria").Value;

        return new Country(id, countryCode, countryName);
    }
}
