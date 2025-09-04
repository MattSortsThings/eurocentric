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

    internal Country(CountryId id, CountryCode countryCode, CountryName countryName) : base(id)
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

    /// <summary>
    ///     Starts the process of creating a new <see cref="Country" /> instance using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="CountryBuilder" /> instance.</returns>
    public static CountryBuilder Create() => new();
}
