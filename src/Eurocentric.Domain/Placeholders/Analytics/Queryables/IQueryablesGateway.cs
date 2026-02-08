namespace Eurocentric.Domain.Placeholders.Analytics.Queryables;

public interface IQueryablesGateway
{
    Task<List<QueryableBroadcast>> GetQueryableBroadcastsAsync(CancellationToken cancellationToken = default);

    Task<List<QueryableContest>> GetQueryableContestsAsync(CancellationToken cancellationToken = default);

    Task<List<QueryableCountry>> GetQueryableCountriesAsync(CancellationToken cancellationToken = default);
}
