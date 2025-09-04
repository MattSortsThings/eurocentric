using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Creates <see cref="CountryId" /> instances on request.
/// </summary>
public interface ICountryIdGenerator
{
    /// <summary>
    ///     Creates and returns a new <see cref="CountryId" /> instance.
    /// </summary>
    /// <returns>A new <see cref="CountryId" /> instance.</returns>
    CountryId CreateSingle();
}
