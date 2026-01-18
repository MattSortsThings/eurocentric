using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EFCore;
using Eurocentric.Domain.Aggregates.Placeholders;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Repositories.Placeholders;

internal sealed class PlaceholderCountryRepository(AppDbContext dbContext) : ICountryRepository
{
    public async Task<Maybe<Country>> GetUntrackedAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbContext
            .Set<Country>()
            .AsNoTracking()
            .SingleOrDefaultAsync(country => country.Id == id, cancellationToken);

    public async Task<List<Country>> GetAllUntrackedAsync<TKey>(
        Expression<Func<Country, TKey>> sortKeySelector,
        CancellationToken cancellationToken = default
    ) => await dbContext.Set<Country>().AsNoTracking().OrderBy(sortKeySelector).ToListAsync(cancellationToken);

    public IQueryable<Country> GetUntrackedQueryable() => dbContext.Set<Country>().AsNoTracking();

    public void Add(Country country) => dbContext.Set<Country>().Add(country);

    public void Remove(Country country) => dbContext.Set<Country>().Remove(country);

    public async Task<Maybe<Country>> GetTrackedAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbContext.Set<Country>().SingleOrDefaultAsync(country => country.Id == id, cancellationToken);
}
