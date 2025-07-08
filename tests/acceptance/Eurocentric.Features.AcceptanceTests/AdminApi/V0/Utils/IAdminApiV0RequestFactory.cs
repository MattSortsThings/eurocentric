using Eurocentric.Features.AdminApi.V0.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

public interface IAdminApiV0RequestFactory
{
    public IContestsEndpoints Contests { get; }

    public interface IContestsEndpoints
    {
        public RestRequest CreateContest(CreateContestRequest requestBody);
        public RestRequest GetContest(Guid contestId);
        public RestRequest GetContests();
    }
}
