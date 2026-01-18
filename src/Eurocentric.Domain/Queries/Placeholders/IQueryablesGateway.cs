namespace Eurocentric.Domain.Queries.Placeholders;

public interface IQueryablesGateway
{
    Task<List<QueryableContest>> GetQueryableContestsAsync(CancellationToken cancellationToken = default);

    Task<List<QueryableCountry>> GetQueryableCountriesAsync(CancellationToken cancellationToken = default);
}
