using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Represents a country or pseudo-country.
/// </summary>
public sealed class Country : AggregateRoot<CountryId>
{
    private readonly List<ContestRole> _contestRoles = [];

    [UsedImplicitly(Reason = "EF Core")]
    private Country() { }

    private Country(CountryId id, CountryCode countryCode, CountryName countryName)
    {
        Id = id;
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
    ///     Gets a list of all the country's contest roles.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a copy of the country's contest role list.</remarks>
    public IReadOnlyList<ContestRole> ContestRoles => _contestRoles.ToArray().AsReadOnly();

    /// <summary>
    ///     Adds a new contest role with the specified <see cref="ContestRole.ContestId" /> and a the
    ///     <see cref="ContestRoleType.GlobalTelevote" /> role type.
    /// </summary>
    /// <param name="contestId">The contest ID.</param>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     The <paramref name="contestId" /> argument matches an existing item in the
    ///     <see cref="ContestRoles" /> collection of this instance.
    /// </exception>
    public void AddGlobalTelevoteContestRole(ContestId contestId)
    {
        ArgumentNullException.ThrowIfNull(contestId);
        ThrowOnContestIdConflict(contestId);

        _contestRoles.Add(new ContestRole(contestId, ContestRoleType.GlobalTelevote));
    }

    /// <summary>
    ///     Adds a new contest role with the specified <see cref="ContestRole.ContestId" /> and a the
    ///     <see cref="ContestRoleType.Participant" /> role type.
    /// </summary>
    /// <param name="contestId">The contest ID.</param>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     The <paramref name="contestId" /> argument matches an existing item in the
    ///     <see cref="ContestRoles" /> collection of this instance.
    /// </exception>
    public void AddParticipantContestRole(ContestId contestId)
    {
        ArgumentNullException.ThrowIfNull(contestId);
        ThrowOnContestIdConflict(contestId);

        _contestRoles.Add(new ContestRole(contestId, ContestRoleType.Participant));
    }

    /// <inheritdoc />
    public override IDomainEvent[] DetachAllDomainEvents() => DetachDomainEvents().ToArray();

    /// <summary>
    ///     Starts the process of creating a new <see cref="Country" /> instance using the fluent builder.
    /// </summary>
    /// <returns>A new instance of a type that implements <see cref="ICountryBuilder" />.</returns>
    public static ICountryBuilder Create() => new Builder();

    private void ThrowOnContestIdConflict(ContestId contestId)
    {
        if (_contestRoles.Any(role => role.ContestId.Equals(contestId)))
        {
            throw new ArgumentException("Country already has a ContestRole with the provided ContestId.");
        }
    }

    private sealed class Builder : ICountryBuilder
    {
        private Result<CountryCode, IDomainError> ErrorOrCountryCode { get; set; } =
            CountryErrors.CountryCodePropertyNotSet();

        private Result<CountryName, IDomainError> ErrorOrCountryName { get; set; } =
            CountryErrors.CountryNamePropertyNotSet();

        public ICountryBuilder WithCountryCode(Result<CountryCode, IDomainError> errorOrCountryCode)
        {
            ErrorOrCountryCode = errorOrCountryCode;

            return this;
        }

        public ICountryBuilder WithCountryName(Result<CountryName, IDomainError> errorOrCountryName)
        {
            ErrorOrCountryName = errorOrCountryName;

            return this;
        }

        public Result<Country, IDomainError> Build(Func<CountryId> idProvider)
        {
            ArgumentNullException.ThrowIfNull(idProvider);

            return ValueTuple.Create(ErrorOrCountryCode, ErrorOrCountryName).Combine().Map(Initialize(idProvider));
        }

        private static Func<ValueTuple<CountryCode, CountryName>, Country> Initialize(Func<CountryId> idProvider)
        {
            Func<CountryId> func = idProvider;

            return tuple => new Country(func(), tuple.Item1, tuple.Item2);
        }
    }
}
