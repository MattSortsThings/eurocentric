using Eurocentric.Domain.Placeholders;

namespace Eurocentric.Infrastructure.InMemoryRepositories;

public sealed class InMemoryQueryableRepository
{
    public List<QueryableBroadcast> QueryableBroadcasts { get; init; } = [];

    public List<QueryableContest> QueryableContests { get; init; } = [];

    public List<QueryableCountry> QueryableCountries { get; init; } = [];

    public List<QueryableCompetitor> QueryableCompetitors { get; init; } = [];

    public List<QueryablePointsAward> QueryableJuryPointsAwards { get; init; } = [];

    public List<QueryablePointsAward> QueryableTelevotePointsAwards { get; init; } = [];

    public void Reset()
    {
        QueryableBroadcasts.Clear();
        QueryableContests.Clear();
        QueryableCountries.Clear();
        QueryableCompetitors.Clear();
        QueryableJuryPointsAwards.Clear();
        QueryableTelevotePointsAwards.Clear();
    }
}
