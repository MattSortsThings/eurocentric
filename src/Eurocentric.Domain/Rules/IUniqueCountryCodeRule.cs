using ErrorOr;
using Eurocentric.Domain.Countries;

namespace Eurocentric.Domain.Rules;

/// <summary>
///     Defines a method for enforcing country code uniqueness across all country aggregates.
/// </summary>
public interface IUniqueCountryCodeRule
{
    /// <summary>
    ///     Validates the specified <see cref="Country" /> aggregate to ensure that no existing country aggregate has an equal
    ///     <see cref="Country.CountryCode" /> value.
    /// </summary>
    /// <param name="country">The country aggregate to be validated.</param>
    /// <returns>
    ///     The <paramref name="country" /> argument if it has a unique <see cref="Country.CountryCode" /> value;
    ///     otherwise, a list of errors.
    /// </returns>
    public ErrorOr<Country> Validate(Country country);
}
