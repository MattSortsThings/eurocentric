using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Countries;

/// <summary>
///     Generates a <see cref="CountryId" /> instance on demand.
/// </summary>
public interface ICountryIdGenerator
{
    /// <summary>
    ///     Creates and returns a new <see cref="CountryId" /> instance with a random <see cref="CountryId.Value" />.
    /// </summary>
    /// <returns>A new <see cref="ContestId" /> instance.</returns>
    public CountryId Generate();
}
