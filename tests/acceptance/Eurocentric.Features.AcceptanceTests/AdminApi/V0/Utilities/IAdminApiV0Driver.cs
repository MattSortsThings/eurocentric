using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V0.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utilities;

public interface IAdminApiV0Driver
{
    public IContests Contests { get; }

    public interface IContests
    {
        public Task<ProblemOrResponse<CreateContestResponse>> CreateContest(CreateContestRequest requestBody,
            CancellationToken cancellationToken = default);

        public Task<ProblemOrResponse<GetContestResponse>> GetContest(Guid contestId,
            CancellationToken cancellationToken = default);

        public Task<ProblemOrResponse<GetContestsResponse>> GetContests(CancellationToken cancellationToken = default);
    }
}
