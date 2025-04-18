using ErrorOr;
using Eurocentric.Features.PublicApi.V0.QueryableCountries.Models;
using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.PublicApi.V0.QueryableCountries.GetQueryableCountries;

internal sealed class
    GetQueryableCountriesQueryHandler : IQueryHandler<GetQueryableCountriesQuery, GetQueryableCountriesResponse>
{
    public Task<ErrorOr<GetQueryableCountriesResponse>> OnHandle(GetQueryableCountriesQuery query,
        CancellationToken cancellationToken)
    {
        QueryableCountry[] queryableCountries =
        [
            new("CH", "Switzerland"),
            new("FR", "France"),
            new("GB", "United Kingdom"),
            new("IS", "Iceland")
        ];

        GetQueryableCountriesResponse response = new(queryableCountries.Length, queryableCountries);

        return Task.FromResult(ErrorOrFactory.From(response));
    }
}
