using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
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

    public Country(CountryId id, CountryCode countryCode, CountryName countryName)
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
    ///     Starts the process of creating a new <see cref="Country" /> instance using the fluent builder.
    /// </summary>
    /// <returns>A new instance of a type that implements <see cref="ICountryBuilder" />.</returns>
    public static ICountryBuilder Create() => new Builder();

    private sealed class Builder : ICountryBuilder
    {
        private Result<CountryCode, IDomainError> ErrorOrCountryCode { get; set; } = CountryErrors.CountryCodeNotSet();

        private Result<CountryName, IDomainError> ErrorOrCountryName { get; set; } = CountryErrors.CountryNameNotSet();

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
