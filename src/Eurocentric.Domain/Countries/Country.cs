using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Countries;

/// <summary>
///     Represents a country aggregate.
/// </summary>
public sealed class Country : AggregateRoot<CountryId>
{
    private readonly List<ContestMemo> _contestMemos = [];

    /// <summary>
    ///     Parameterless constructor for EF Core.
    /// </summary>
    private Country() { }

    internal Country(CountryId id, CountryCode countryCode, CountryName name) : base(id)
    {
        CountryCode = countryCode;
        Name = name;
    }

    /// <summary>
    ///     Gets the country's ISO 3166-1 alpha-2 code.
    /// </summary>
    public CountryCode CountryCode { get; private init; } = null!;

    /// <summary>
    ///     Gets the country's short UK English name.
    /// </summary>
    public CountryName Name { get; private init; } = null!;

    /// <summary>
    ///     Gets a list of memoized contest aggregates in which the country is a participant, ordered by contest ID value.
    /// </summary>
    public IReadOnlyList<ContestMemo> ContestMemos => _contestMemos
        .OrderBy(memo => memo.ContestId.Value)
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Adds a new <see cref="ContestMemo" /> to this instance's <see cref="ContestMemos" /> collection, with the
    ///     provided <see cref="ContestMemo.ContestId" /> value and a <see cref="ContestMemo.Status" /> value of
    ///     <see cref="ContestStatus.Initialized" />.
    /// </summary>
    /// <param name="contestId">Identifies the contest aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     This instance contains a <see cref="ContestMemo" /> instance with a
    ///     <see cref="ContestMemo.ContestId" /> value equal to the <paramref name="contestId" /> argument.
    /// </exception>
    public void AddMemo(ContestId contestId)
    {
        ArgumentNullException.ThrowIfNull(contestId);

        if (_contestMemos.Any(memo => memo.ContestId == contestId))
        {
            throw new ArgumentException("ContestMemos collection contains an item with the provided ContestId value.");
        }

        _contestMemos.Add(new ContestMemo(contestId, ContestStatus.Initialized));
    }

    /// <summary>
    ///     Replaces an existing <see cref="ContestMemo" /> in this instance's <see cref="ContestMemos" /> collection.
    /// </summary>
    /// <param name="contestId">Identifies the contest aggregate.</param>
    /// <param name="status">The current status of the contest aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     This instance contains no <see cref="ContestMemo" /> instance with a
    ///     <see cref="ContestMemo.ContestId" /> value equal to the <paramref name="contestId" /> argument.
    /// </exception>
    public void ReplaceMemo(ContestId contestId, ContestStatus status)
    {
        ArgumentNullException.ThrowIfNull(contestId);

        ContestMemo removed = RemoveExistingMemoOrThrowIfNotFound(contestId);
        _contestMemos.Add(new ContestMemo(removed.ContestId, status));
    }

    /// <summary>
    ///     Removes an existing <see cref="ContestMemo" /> from this instance's <see cref="ContestMemos" /> collection.
    /// </summary>
    /// <param name="contestId">Identifies the contest aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     This instance contains no <see cref="ContestMemo" /> instance with a
    ///     <see cref="ContestMemo.ContestId" /> value equal to the <paramref name="contestId" /> argument.
    /// </exception>
    public void RemoveMemo(ContestId contestId)
    {
        ArgumentNullException.ThrowIfNull(contestId);

        _ = RemoveExistingMemoOrThrowIfNotFound(contestId);
    }

    /// <summary>
    ///     Starts the process of building a new <see cref="Country" /> aggregate using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="CountryBuilder" /> instance.</returns>
    public static CountryBuilder Create() => new();

    private ContestMemo RemoveExistingMemoOrThrowIfNotFound(ContestId contestId)
    {
        if (_contestMemos.SingleOrDefault(memo => memo.ContestId == contestId) is not { } existingMemo)
        {
            throw new ArgumentException("ContestMemos collection contains no item with the provided ContestId value.");
        }

        _contestMemos.Remove(existingMemo);

        return existingMemo;
    }
}
