using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Represents a country aggregate.
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
    ///     Gets the country's ISO 3166-1 alpha-2 country code
    /// </summary>
    public CountryCode CountryCode { get; private init; } = null!;

    /// <summary>
    ///     Gets the country's short UK English name.
    /// </summary>
    public CountryName CountryName { get; private init; } = null!;

    /// <summary>
    ///     Gets a list of the IDs of all contest aggregates in which the country has a participant.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a new list populated from the instance's private data.</remarks>
    public IReadOnlyList<ContestId> ParticipatingContestIds => _participatingContestIds
        .ToArray()
        .AsReadOnly();

    /// <inheritdoc />
    public override IDomainEvent[] DetachAllDomainEvents() => DetachDomainEvents().ToArray();

    /// <summary>
    ///     Adds a copy of the provided <see cref="ContestId" /> value to the contest's <see cref="ParticipatingContestIds" />
    ///     collection.
    /// </summary>
    /// <param name="contestId">The ID of the contest aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     This instance already contains a <see cref="ContestId" /> value equal to the <paramref name="contestId" />
    ///     argument.
    /// </exception>
    public void AddParticipatingContestId(ContestId contestId)
    {
        ArgumentNullException.ThrowIfNull(contestId);

        if (_participatingContestIds.Any(id => id.Equals(contestId)))
        {
            throw new ArgumentException("Country ParticipatingContestIds collection already contains provided ContestId value.");
        }

        _participatingContestIds.Add(ContestId.FromValue(contestId.Value));
    }

    /// <summary>
    ///     Removes the provided <see cref="ContestId" /> value from the contest's <see cref="ParticipatingContestIds" />
    ///     collection.
    /// </summary>
    /// <param name="contestId">The ID of the contest aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     This instance does not contain a <see cref="ContestId" /> value equal to the <paramref name="contestId" />
    ///     argument.
    /// </exception>
    public void RemoveParticipatingContestId(ContestId contestId)
    {
        ArgumentNullException.ThrowIfNull(contestId);

        if (_participatingContestIds.SingleOrDefault(id => id.Equals(contestId)) is not { } idToRemove)
        {
            throw new ArgumentException("Country ParticipatingContestIds collection does not contain provided ContestId value.");
        }

        _participatingContestIds.Remove(idToRemove);
    }

    /// <summary>
    ///     Starts the process of creating a new <see cref="Country" /> instance using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="CountryBuilder" /> instance.</returns>
    public static CountryBuilder Create() => new();
}
