using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Represents a country aggregate.
/// </summary>
public sealed class Country : AggregateRoot<CountryId>
{
    private readonly List<ContestId> _participatingContestIds = [];

    private Country()
    {
    }

    public Country(CountryId id, CountryCode countryCode, CountryName name) : base(id)
    {
        CountryCode = countryCode;
        CountryName = name;
    }

    /// <summary>
    ///     Gets the country's ISO 3166-1 alpha-2 code.
    /// </summary>
    public CountryCode CountryCode { get; private init; } = null!;

    /// <summary>
    ///     Gets the country's short UK English name.
    /// </summary>
    public CountryName CountryName { get; private init; } = null!;

    /// <summary>
    ///     Gets an ordered list of the IDs of all the contest aggregates in which the country is a participant.
    /// </summary>
    public IReadOnlyList<ContestId> ParticipatingContestIds => _participatingContestIds
        .OrderBy(id => id.Value)
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Starts the process of building a new <see cref="Country" /> instance using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="CountryBuilder" /> instance.</returns>
    public static CountryBuilder Create() => new();
}
