using ErrorOr;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Extension methods to enforce invariants across all existing <see cref="Country" /> aggregates.
/// </summary>
public static class InvariantEnforcement
{
    /// <summary>
    ///     Fails if the newly instantiated <see cref="Country" /> aggregate has the same <see cref="Country.CountryCode" />
    ///     value as an existing <see cref="Country" /> aggregate.
    /// </summary>
    /// <param name="errorsOrCountry">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i>
    ///     a <see cref="Country" /> object.
    /// </param>
    /// <param name="existingCountries">All the existing <see cref="Country" /> aggregates in the system.</param>
    /// <returns>
    ///     A new list of <see cref="Error" /> objects if the <paramref name="errorsOrCountry" /> argument is a
    ///     <see cref="Country" /> and its <see cref="Country.CountryCode" /> is not unique; otherwise, the
    ///     <paramref name="errorsOrCountry" /> argument is returned.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="existingCountries" /> is <see langword="null" />.</exception>
    public static ErrorOr<Country> FailOnCountryCodeConflict(this ErrorOr<Country> errorsOrCountry,
        IQueryable<Country> existingCountries)
    {
        ArgumentNullException.ThrowIfNull(existingCountries);

        if (errorsOrCountry.IsError)
        {
            return errorsOrCountry;
        }

        CountryCode countryCode = errorsOrCountry.Value.CountryCode;

        return existingCountries.Any(country => country.CountryCode == countryCode)
            ? CountryErrors.CountryCodeConflict(countryCode)
            : errorsOrCountry;
    }
}
