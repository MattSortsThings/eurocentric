using Eurocentric.Domain.Abstractions;
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
    ///     Adds the specified <see cref="ContestMemo" /> value to this instance's <see cref="ContestMemos" /> collection,
    ///     replacing any existing item with the same <see cref="ContestMemo.ContestId" /> value.
    /// </summary>
    /// <param name="contestMemo">The memo to be added.</param>
    /// <exception cref="ArgumentNullException"><paramref name="contestMemo" /> is <see langword="null" />.</exception>
    public void AddOrReplaceMemo(ContestMemo contestMemo)
    {
        ArgumentNullException.ThrowIfNull(contestMemo);

        if (_contestMemos.FirstOrDefault(memo => memo.ContestId == contestMemo.ContestId) is { } existingMemo)
        {
            _contestMemos.Remove(existingMemo);
        }

        _contestMemos.Add(contestMemo);
    }

    /// <summary>
    ///     Removes the memo with the provided <see cref="ContestMemo.ContestId" /> value from this instance's
    ///     <see cref="ContestMemos" /> collection.
    /// </summary>
    /// <param name="contestId">The <see cref="ContestMemo.ContestId" /> value of the memo to be removed.</param>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">No memo matches the <paramref name="contestId" /> parameter.</exception>
    public void RemoveMemo(ContestId contestId)
    {
        ArgumentNullException.ThrowIfNull(contestId);

        if (_contestMemos.FirstOrDefault(memo => memo.ContestId == contestId) is { } existingMemo)
        {
            _contestMemos.Remove(existingMemo);
        }
        else
        {
            throw new ArgumentException($"No contest memo present with contest ID {contestId.Value}.");
        }
    }

    /// <summary>
    ///     Starts the process of building a new <see cref="Country" /> aggregate using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="CountryBuilder" /> instance.</returns>
    public static CountryBuilder Create() => new();
}
