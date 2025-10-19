namespace Eurocentric.Domain.V0.Queries.Queryables;

public interface IQueryablesGateway
{
    Task<QueryableBroadcast[]> GetQueryableBroadcastsAsync(CancellationToken cancellationToken = default);

    Task<QueryableContest[]> GetQueryableContestsAsync(CancellationToken cancellationToken = default);

    Task<QueryableCountry[]> GetQueryableCountriesAsync(CancellationToken cancellationToken = default);
}
