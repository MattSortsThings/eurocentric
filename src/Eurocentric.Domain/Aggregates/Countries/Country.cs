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
}
