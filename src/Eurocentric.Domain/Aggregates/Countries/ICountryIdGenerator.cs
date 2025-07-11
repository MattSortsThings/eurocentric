using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Generates <see cref="CountryId" /> object.
/// </summary>
public interface ICountryIdGenerator
{
    /// <summary>
    ///     Creates and returns a new <see cref="CountryId" /> object.
    /// </summary>
    /// <returns>A new <see cref="CountryId" /> instance.</returns>
    public CountryId CreateSingle();
}
