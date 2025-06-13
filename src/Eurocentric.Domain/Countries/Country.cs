using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Countries;

/// <summary>
///     Represents a country aggregate.
/// </summary>
public sealed class Country : AggregateRoot<CountryId>
{
    private readonly List<ContestMemo> _participatingContests = [];

    [UsedImplicitly]
    private Country()
    {
    }

    internal Country(CountryId id, CountryCode countryCode, CountryName countryName) : base(id)
    {
        CountryCode = countryCode;
        CountryName = countryName;
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
    ///     Gets a list of memos of all contest aggregates in which the country has a participant, ordered by contest ID value.
    /// </summary>
    /// <remarks>The list is newly generated every time this property is accessed.</remarks>
    public IReadOnlyList<ContestMemo> ParticipatingContests => _participatingContests
        .OrderBy(memo => memo.ContestId.Value)
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Adds a new <see cref="ContestMemo" /> to this instance's <see cref="ParticipatingContests" /> collection, with the
    ///     provided <see cref="ContestMemo.ContestId" /> value and a <see cref="ContestMemo.ContestStatus" /> value of
    ///     <see cref="ContestStatus.Initialized" />.
    /// </summary>
    /// <param name="contestId">The ID of the contest aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     This instance's <see cref="ParticipatingContests" /> collection already contains a
    ///     <see cref="ContestMemo" /> with the <paramref name="contestId" /> argument.
    /// </exception>
    public void AddMemo(ContestId contestId)
    {
        ArgumentNullException.ThrowIfNull(contestId);

        if (_participatingContests.Any(memo => memo.ContestId == contestId))
        {
            throw new ArgumentException("ContestMemo already exists with the provided ContestId value.");
        }

        _participatingContests.Add(new ContestMemo(contestId, ContestStatus.Initialized));
    }

    /// <summary>
    ///     Begins the process of creating a new <see cref="Country" /> instance using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="CountryBuilder" /> instance.</returns>
    public static CountryBuilder Create() => new();
}
