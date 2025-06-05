using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Countries;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

public interface IAdminApiV1Driver
{
    public ICountries Countries { get; }

    public interface ICountries
    {
        public Task<ProblemOrResponse<GetCountryResponse>> GetCountry(Guid countryId,
            CancellationToken cancellationToken = default);
    }
}
