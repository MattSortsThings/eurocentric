using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

internal static partial class AdminApiV1DriverExtensions
{
    internal static async Task<Contest> CreateAContestAsync(this IAdminApiV1Driver.IContests driver,
        int contestYear = 0,
        string cityName = "",
        ContestFormat contestFormat = ContestFormat.Stockholm,
        Guid? group0CountryId = null,
        IEnumerable<Guid>? group1CountryIds = null,
        IEnumerable<Guid>? group2CountryIds = null,
        CancellationToken cancellationToken = default)
    {
        CreateContestRequest requestBody = new()
        {
            ContestYear = contestYear,
            CityName = cityName,
            ContestFormat = contestFormat,
            Group0CountryId = group0CountryId,
            Group1Participants = group1CountryIds?.ToParticipantSpecifications() ?? [],
            Group2Participants = group2CountryIds?.ToParticipantSpecifications() ?? []
        };

        ProblemOrResponse<CreateContestResponse> problemOrResponse =
            await driver.CreateContest(requestBody, cancellationToken);

        return problemOrResponse.AsResponse.Data!.Contest;
    }

    internal static async Task<Broadcast> CreateAChildBroadcastAsync(this IAdminApiV1Driver.IContests driver,
        Guid[]? competingCountryIds = null,
        Guid contestId = default,
        ContestStage contestStage = default,
        DateOnly broadcastDate = default,
        CancellationToken cancellationToken = default)
    {
        CreateChildBroadcastRequest requestBody = new()
        {
            ContestStage = contestStage, BroadcastDate = broadcastDate, CompetingCountryIds = competingCountryIds ?? []
        };

        ProblemOrResponse<CreateChildBroadcastResponse> problemOrResponse =
            await driver.CreateChildBroadcast(contestId, requestBody, cancellationToken);

        return problemOrResponse.AsResponse.Data!.Broadcast;
    }

    internal static async Task DeleteAContestAsync(this IAdminApiV1Driver.IContests driver,
        Guid contestId,
        CancellationToken cancellationToken = default) => _ = await driver.DeleteContest(contestId, cancellationToken);

    internal static async Task<Contest> GetAContestAsync(this IAdminApiV1Driver.IContests driver,
        Guid contestId,
        CancellationToken cancellationToken = default)
    {
        ProblemOrResponse<GetContestResponse> problemOrResponse = await driver.GetContest(contestId, cancellationToken);

        return problemOrResponse.AsResponse.Data!.Contest;
    }

    internal static async Task<Contest[]> GetAllContestAsync(this IAdminApiV1Driver.IContests driver,
        CancellationToken cancellationToken = default)
    {
        ProblemOrResponse<GetContestsResponse> problemOrResponse = await driver.GetContests(cancellationToken);

        return problemOrResponse.AsResponse.Data!.Contests;
    }
}
