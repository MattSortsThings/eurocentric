using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Features.AdminApi.V1.Countries;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

public interface IAdminApiV1Driver
{
    public IContests Contests { get; }

    public ICountries Countries { get; }

    public interface IContests
    {
        public Task<ProblemOrResponse<GetContestResponse>> GetContest(Guid contestId,
            CancellationToken cancellationToken = default);
    }

    public interface ICountries
    {
        public Task<ProblemOrResponse<CreateCountryResponse>> CreateCountry(CreateCountryRequest requestBody,
            CancellationToken cancellationToken = default);

        public Task<ProblemOrResponse<GetCountriesResponse>> GetCountries(CancellationToken cancellationToken = default);

        public Task<ProblemOrResponse<GetCountryResponse>> GetCountry(Guid countryId,
            CancellationToken cancellationToken = default);
    }
}
