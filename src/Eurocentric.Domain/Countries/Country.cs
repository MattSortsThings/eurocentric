using ErrorOr;
using Eurocentric.Domain.BaseTypes;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Utils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Countries;

/// <summary>
///     Represents a country aggregate.
/// </summary>
public sealed class Country : AggregateRoot<CountryId>
{
    private readonly List<ContestId> _contestIds = [];

    private Country()
    {
    }

    private Country(CountryId id, CountryCode countryCode, CountryName countryName, CountryType countryType) : base(id)
    {
        CountryCode = countryCode;
        CountryName = countryName;
        CountryType = countryType;
    }

    /// <summary>
    ///     Gets this country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public CountryCode CountryCode { get; private set; } = null!;

    /// <summary>
    ///     Gets this country's short UK English name.
    /// </summary>
    public CountryName CountryName { get; private set; } = null!;

    /// <summary>
    ///     Gets this country's type.
    /// </summary>
    public CountryType CountryType { get; private set; }

    /// <summary>
    ///     Gets a collection of the IDs of all the contest aggregates in which this country aggregate is involved.
    /// </summary>
    public IReadOnlyList<ContestId> ContestIds => _contestIds.OrderBy(id => id.Value).ToArray().AsReadOnly();

    /// <summary>
    ///     Adds the specified <see cref="ContestId" /> value to this instance's <see cref="ContestIds" /> collection.
    /// </summary>
    /// <param name="contestId">The contest ID to be added.</param>
    /// <exception cref="ArgumentException">
    ///     A value equal to <paramref name="contestId" /> is already present in this instance's <see cref="ContestIds" />
    ///     collection.
    /// </exception>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    public void AddContestId(ContestId contestId)
    {
        ArgumentNullException.ThrowIfNull(contestId);

        if (_contestIds.Any(existing => existing.Equals(contestId)))
        {
            throw new ArgumentException($"ContestId {contestId.Value} is already present in country {Id.Value}.");
        }

        _contestIds.Add(contestId);
    }

    /// <summary>
    ///     Removes the specified <see cref="ContestId" /> value from this instance's <see cref="ContestIds" /> collection.
    /// </summary>
    /// <param name="contestId">The contest ID to be removed.</param>
    /// <exception cref="ArgumentException">
    ///     No value equal to <paramref name="contestId" /> is present in this instance's <see cref="ContestIds" /> collection.
    /// </exception>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    public void RemoveContestId(ContestId contestId)
    {
        ArgumentNullException.ThrowIfNull(contestId);

        if (_contestIds.FirstOrDefault(existing => existing.Equals(contestId)) is var idToRemove && idToRemove is null)
        {
            throw new ArgumentException($"ContestId {contestId.Value} is not present in country {Id.Value}.");
        }

        _contestIds.Remove(idToRemove);
    }

    /// <summary>
    ///     Starts the process of building a new <see cref="Country" /> aggregate with a <see cref="CountryType" /> value of
    ///     <see cref="Enums.CountryType.Real" />.
    /// </summary>
    /// <returns>A new fluent builder instance.</returns>
    public static ICountryBuilder CreateReal() => new Builder(CountryType.Real);

    /// <summary>
    ///     Starts the process of building a new <see cref="Country" /> aggregate with a <see cref="CountryType" /> value of
    ///     <see cref="Enums.CountryType.Pseudo" />.
    /// </summary>
    /// <returns>A new fluent builder instance.</returns>
    public static ICountryBuilder CreatePseudo() => new Builder(CountryType.Pseudo);

    private sealed class Builder : ICountryBuilder, ICountryFinisher, ICountryNameSetter
    {
        private readonly CountryType _countryType;
        private ErrorOr<CountryCode> _errorOrCountryCode;
        private ErrorOr<CountryName> _errorOrCountryName;

        public Builder(CountryType countryType)
        {
            _countryType = countryType;
        }

        public ICountryNameSetter WithCountryCode(string countryCode)
        {
            ArgumentNullException.ThrowIfNull(countryCode);

            _errorOrCountryCode = CountryCode.Create(countryCode);

            return this;
        }

        public ErrorOr<Country> Build(DateTimeOffset dateTimeOffset) =>
            ErrorOrTupleFactory.Combine(_errorOrCountryCode, _errorOrCountryName)
                .Then(tuple => CreateCountry(tuple.Item1, tuple.Item2, dateTimeOffset));

        public ICountryFinisher AndCountryName(string countryName)
        {
            ArgumentNullException.ThrowIfNull(countryName);

            _errorOrCountryName = CountryName.Create(countryName);

            return this;
        }

        private Country CreateCountry(CountryCode countryCode, CountryName countryName, DateTimeOffset dateTimeOffset)
        {
            CountryId id = CountryId.Create(dateTimeOffset);

            return new Country(id, countryCode, countryName, _countryType);
        }
    }
}
