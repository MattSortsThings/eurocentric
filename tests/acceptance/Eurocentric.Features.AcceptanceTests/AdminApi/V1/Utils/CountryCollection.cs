using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public sealed class CountryCollection
{
    private readonly Dictionary<string, Country> _countryCodeLookup = new(6, StringComparer.Ordinal);

    /// <summary>
    ///     Adds the provided country to the collection.
    /// </summary>
    /// <param name="country">The country to be added.</param>
    public void Add(Country country) => _countryCodeLookup.Add(country.CountryCode, country);

    /// <summary>
    ///     Enumerates all the countries in the collection.
    /// </summary>
    /// <remarks>No assumptions should be made regarding the order of the countries returned by this method.</remarks>
    /// <returns>A sequence of countries.</returns>
    public IEnumerable<Country> GetAll() => _countryCodeLookup.Values;

    /// <summary>
    ///     Retrieves the single country in the collection.
    /// </summary>
    /// <returns>A country.</returns>
    public Country GetSingle() => _countryCodeLookup.Values.Single();

    /// <summary>
    ///     Gets the ID of the country in the collection with the provided country code value.
    /// </summary>
    /// <param name="countryCode">The country code to be queried.</param>
    /// <returns>A country.</returns>
    public Guid LookupId(string countryCode) => _countryCodeLookup[countryCode].Id;

    /// <summary>
    ///     Resizes the internal data structures to ensure it can hold up to the specified number of countries without needing
    ///     to resize.
    /// </summary>
    /// <param name="capacity">The number of items to be stored.</param>
    public void EnsureCapacity(int capacity) => _countryCodeLookup.EnsureCapacity(capacity);
}
