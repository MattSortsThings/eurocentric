using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Features.AdminApi.V1.Countries;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

public interface IAdminApiV1Driver
{
    public IBroadcasts Broadcasts { get; }

    public IContests Contests { get; }

    public ICountries Countries { get; }

    public interface IBroadcasts
    {
        public Task<ProblemOrResponse> AwardTelevotePoints(Guid broadcastId,
            AwardTelevotePointsRequest requestBody,
            CancellationToken cancellationToken = default);

        public Task<ProblemOrResponse<GetBroadcastResponse>> GetBroadcast(Guid broadcastId,
            CancellationToken cancellationToken = default);

        public Task<ProblemOrResponse<GetBroadcastsResponse>> GetBroadcasts(CancellationToken cancellationToken = default);
    }

    public interface IContests
    {
        public Task<ProblemOrResponse<CreateContestResponse>> CreateContest(CreateContestRequest requestBody,
            CancellationToken cancellationToken = default);

        public Task<ProblemOrResponse<CreateChildBroadcastResponse>> CreateChildBroadcast(Guid contestId,
            CreateChildBroadcastRequest requestBody,
            CancellationToken cancellationToken = default);

        public Task<ProblemOrResponse<GetContestResponse>> GetContest(Guid contestId,
            CancellationToken cancellationToken = default);

        public Task<ProblemOrResponse<GetContestsResponse>> GetContests(CancellationToken cancellationToken = default);
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
