using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.V0.Aggregates.Countries;

public interface ICountryReadRepository
{
    Task<Country[]> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<Country, IDomainError>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    IQueryable<Country> GetQueryable();
}
