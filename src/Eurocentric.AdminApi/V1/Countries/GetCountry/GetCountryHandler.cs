using ErrorOr;
using Eurocentric.AdminApi.V1.Countries.Models;
using Eurocentric.DataAccess.InMemory;
using Eurocentric.Domain.DomainErrors;
using Eurocentric.Shared.AppPipeline;
using Country = Eurocentric.Domain.Countries.Country;

namespace Eurocentric.AdminApi.V1.Countries.GetCountry;

internal sealed class GetCountryHandler(InMemoryRepository repository) : QueryHandler<GetCountryQuery, GetCountryResult>
{
    public override async Task<ErrorOr<GetCountryResult>> Handle(GetCountryQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        Country? country = repository.Countries.FirstOrDefault(country => country.Id.Value == query.CountryId);

        return country is not null
            ? new GetCountryResult(country.ToModelCountry())
            : Errors.Countries.CountryNotFound(query.CountryId);
    }
}
