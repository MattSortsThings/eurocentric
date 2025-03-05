using ErrorOr;
using Eurocentric.AdminApi.V1.Countries.Models;
using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.AdminApi.V1.Countries.GetCountry;

internal sealed class GetCountryHandler : QueryHandler<GetCountryQuery, GetCountryResult>
{
    public override async Task<ErrorOr<GetCountryResult>> Handle(GetCountryQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        Country country = new(query.CountryId,
            "GB",
            "United Kingdom",
            CountryType.Real,
            [Guid.NewGuid(), Guid.NewGuid()]);

        return new GetCountryResult(country);
    }
}
