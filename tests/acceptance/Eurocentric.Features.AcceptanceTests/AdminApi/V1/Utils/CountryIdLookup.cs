using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

/// <summary>
///     Stores country code and ID key-value pairs.
/// </summary>
public sealed class CountryIdLookup
{
    private readonly Dictionary<string, Guid> _dictionary = new(8);

    /// <summary>
    ///     Populates this instance from the provided countries.
    /// </summary>
    /// <param name="countries">The countries from which the instance is to be populated.</param>
    public void Populate(ICollection<Country> countries)
    {
        _dictionary.Clear();
        _dictionary.EnsureCapacity(countries.Count);
        foreach (Country country in countries)
        {
            _dictionary.Add(country.CountryCode, country.Id);
        }
    }

    /// <summary>
    ///     Retrieves the country ID value matching the provided country code value.
    /// </summary>
    /// <param name="countryCode">The unique country code.</param>
    /// <returns>A <see cref="Guid" /> country ID value.</returns>
    public Guid GetSingle(string countryCode) => _dictionary[countryCode];

    /// <summary>
    ///     Creates and returns an array containing all the country ID values stored in the instance.
    /// </summary>
    /// <returns>An array of <see cref="Guid" /> country ID values.</returns>
    public Guid[] GetAll() => _dictionary.Values.ToArray();

    /// <summary>
    ///     Removes the instance entry matching the provided country code value and returns the country ID value.
    /// </summary>
    /// <param name="countryCode">The unique country code.</param>
    /// <returns>A <see cref="Guid" /> country ID value.</returns>
    public Guid RemoveSingle(string countryCode)
    {
        Guid removedId = _dictionary[countryCode];
        _dictionary.Remove(countryCode);

        return removedId;
    }
}
