using Eurocentric.Domain.Countries;

namespace Eurocentric.Domain.Rules.External.DbCheckers;

/// <summary>
///     Methods for checking a new <see cref="Country" /> aggregate against all existing data in the system.
/// </summary>
/// <remarks>Register an implementation of this instance as a scoped service in the data access assembly.</remarks>
public interface ICountryDbChecker
{
    /// <summary>
    ///     Determines whether the system contains an existing <see cref="Country" /> aggregate with a
    ///     <see cref="Country.CountryCode" /> value equal to the specified country.
    /// </summary>
    /// <param name="country">A <see cref="Country" /> aggregate that has not yet been persisted to the system.</param>
    /// <returns>
    ///     <see langword="true" /> if the <paramref name="country" /> argument does not have a unique
    ///     <see cref="Country.CountryCode" /> value; otherwise, <see langword="false" />.
    /// </returns>
    public bool CountryCodeIsNotUnique(Country country);
}
