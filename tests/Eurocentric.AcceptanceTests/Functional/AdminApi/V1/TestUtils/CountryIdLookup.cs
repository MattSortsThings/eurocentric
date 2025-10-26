namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed class CountryIdLookup
{
    private readonly Dictionary<string, Guid> _lookup = new();

    public void EnsureCapacity(int capacity) => _lookup.EnsureCapacity(capacity);

    public void Add(string countryCode, Guid countryId) => _lookup.Add(countryCode, countryId);

    public Guid Remove(string countryCode)
    {
        Guid id = _lookup[countryCode];
        _lookup.Remove(countryCode);

        return id;
    }

    public Guid GetId(string countryCode) => _lookup[countryCode];

    public Guid[] GetAllIds() => _lookup.Values.ToArray();
}
