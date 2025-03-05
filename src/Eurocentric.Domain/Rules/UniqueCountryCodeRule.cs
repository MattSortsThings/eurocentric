using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.DomainErrors;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Rules;

/// <summary>
///     Implements a method for enforcing country code uniqueness across all country aggregates.
/// </summary>
/// <remarks>Register a concrete derivative of this class in the data access project.</remarks>
public abstract class UniqueCountryCodeRule : IUniqueCountryCodeRule
{
    public ErrorOr<Country> Validate(Country country) =>
        Exists(country.CountryCode)
            ? Errors.Countries.CountryCodeConflict(country.CountryCode.Value)
            : country;

    /// <summary>
    ///     Determines whether any existing <see cref="Country" /> aggregate has a <see cref="Country.CountryCode" /> value
    ///     equal to the specified value.
    /// </summary>
    /// <param name="countryCode">The country code value to be queried.</param>
    /// <returns>
    ///     <see langword="true" /> if any existing <see cref="Country" /> aggregate has a
    ///     <see cref="Country.CountryCode" /> value equal to the <paramref name="countryCode" /> argument; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    protected abstract bool Exists(CountryCode countryCode);
}
