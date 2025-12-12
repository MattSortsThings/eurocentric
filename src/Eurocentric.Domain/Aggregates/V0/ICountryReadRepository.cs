using CSharpFunctionalExtensions;
using Eurocentric.Domain.Abstractions.Errors;

namespace Eurocentric.Domain.Aggregates.V0;

public interface ICountryReadRepository
{
    Task<Result<Country, IDomainError>> GetUntrackedAsync(
        Guid countryId,
        CancellationToken cancellationToken = default
    );

    Task<List<Country>> GetAllUntrackedAsync(CancellationToken cancellationToken = default);

    IQueryable<Country> GetUntrackedQueryable();
}
