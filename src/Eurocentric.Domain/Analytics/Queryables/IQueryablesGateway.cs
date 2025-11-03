namespace Eurocentric.Domain.Analytics.Queryables;

public interface IQueryablesGateway
{
    Task<QueryableContest[]> GetQueryableContestsAsync(CancellationToken cancellationToken = default);

    Task<QueryableCountry[]> GetQueryableCountriesAsync(CancellationToken cancellationToken = default);
}
