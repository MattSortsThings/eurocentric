using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

public interface ICountryIdFactory
{
    /// <summary>
    ///     Creates and returns a new <see cref="CountryId" /> instance.
    /// </summary>
    /// <returns>A new <see cref="CountryId" /> instance.</returns>
    CountryId Create();
}
