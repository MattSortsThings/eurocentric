namespace Eurocentric.Domain.Analytics.Queryables;

public interface IQueryablesGateway
{
    Task<QueryableBroadcast[]> GetQueryableBroadcastsAsync(CancellationToken cancellationToken = default);

    Task<QueryableContest[]> GetQueryableContestsAsync(CancellationToken cancellationToken = default);

    Task<QueryableCountry[]> GetQueryableCountriesAsync(CancellationToken cancellationToken = default);
}
