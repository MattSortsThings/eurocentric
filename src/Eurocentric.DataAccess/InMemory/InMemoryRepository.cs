using Eurocentric.Domain.Countries;

namespace Eurocentric.DataAccess.InMemory;

public sealed class InMemoryRepository
{
    public List<Country> Countries { get; } = [];
}
