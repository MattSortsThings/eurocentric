using Eurocentric.Domain.Placeholders;

namespace Eurocentric.Infrastructure.InMemoryRepositories;

public sealed class InMemoryContestRepository
{
    public List<Contest> Contests { get; } = [];

    public void Reset() => Contests.Clear();
}
