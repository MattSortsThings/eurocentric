using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Countries;

/// <summary>
///     Generates <see cref="CountryId" /> value objects.
/// </summary>
public interface ICountryIdProvider
{
    /// <summary>
    ///     Creates and returns a new <see cref="CountryId" /> instance with a unique <see cref="CountryId.Value" />.
    /// </summary>
    /// <returns>A new <see cref="CountryId" /> instance.</returns>
    public CountryId Create();
}
