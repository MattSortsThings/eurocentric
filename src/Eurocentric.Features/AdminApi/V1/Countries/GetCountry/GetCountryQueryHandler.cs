using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Models;
using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.AdminApi.V1.Countries.GetCountry;

internal sealed class GetCountryQueryHandler : RequestHandler<GetCountryQuery, GetCountryResponse>
{
    public override Task<ErrorOr<GetCountryResponse>> OnHandle(GetCountryQuery request,
        CancellationToken cancellationToken = default)
    {
        Country country = new()
        {
            Id = request.CountryId,
            CountryCode = "GB",
            CountryName = "United Kingdom",
            ContestIds =
            [
                Guid.Parse("40d8907b-6031-4e20-9adf-e258202c148b"),
                Guid.Parse("b1c6ce46-f8b5-42ad-9ac9-025393b00dde")
            ]
        };

        return Task.FromResult(ErrorOrFactory.From(new GetCountryResponse(country)));
    }
}
