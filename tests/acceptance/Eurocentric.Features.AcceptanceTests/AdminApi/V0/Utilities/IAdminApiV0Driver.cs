using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V0.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utilities;

public interface IAdminApiV0Driver
{
    public IContests Contests { get; }

    public interface IContests
    {
        public Task<ResponseOrProblem<CreateContestResponse>> CreateContest(CreateContestRequest requestBody,
            CancellationToken cancellationToken = default);

        public Task<ResponseOrProblem<GetContestResponse>> GetContest(Guid contestId,
            CancellationToken cancellationToken = default);

        public Task<ResponseOrProblem<GetContestsResponse>> GetContests(CancellationToken cancellationToken = default);
    }
}
