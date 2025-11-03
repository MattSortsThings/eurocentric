namespace Eurocentric.Domain.Analytics.Queryables;

public interface IQueryablesGateway
{
    Task<QueryableCountry[]> GetQueryableCountriesAsync(CancellationToken cancellationToken = default);
}
