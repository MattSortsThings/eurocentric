using System.Linq.Expressions;
using CSharpFunctionalExtensions;

namespace Eurocentric.Domain.Aggregates.Placeholders;

public interface ICountryReadRepository
{
    Task<Maybe<Country>> GetUntrackedAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Country>> GetAllUntrackedAsync<TKey>(
        Expression<Func<Country, TKey>> sortKeySelector,
        CancellationToken cancellationToken = default
    );

    IQueryable<Country> GetUntrackedQueryable();
}
