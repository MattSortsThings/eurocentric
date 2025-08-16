using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Creates a <see cref="CountryId" /> instance on demand.
/// </summary>
public interface ICountryIdProvider
{
    /// <summary>
    ///     Creates and returns a new <see cref="CountryId" /> instance with a <see cref="CountryId.Value" /> generated
    ///     according to RFC 9562, following the Version 7 format
    /// </summary>
    /// <returns>A new <see cref="CountryId" /> object.</returns>
    CountryId CreateSingle();
}
