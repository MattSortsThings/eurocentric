using Eurocentric.Features.AdminApi.V1.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

/// <summary>
///     Creates REST requests for "Contests" tagged endpoints.
/// </summary>
public interface IContestsRequestFactory
{
    /// <summary>
    ///     Creates a REST request for the "CreateContest" endpoint.
    /// </summary>
    /// <param name="requestBody">The request body.</param>
    /// <returns>A <see cref="RestRequest" /> instance.</returns>
    public RestRequest CreateContest(CreateContestRequest requestBody);

    /// <summary>
    ///     Creates a REST request for the "GetContest" endpoint.
    /// </summary>
    /// <param name="contestId">The contest ID.</param>
    /// <returns>A <see cref="RestRequest" /> instance.</returns>
    public RestRequest GetContest(Guid contestId);

    /// <summary>
    ///     Creates a REST request for the "GetContests" endpoint.
    /// </summary>
    /// <returns>A <see cref="RestRequest" /> instance.</returns>
    public RestRequest GetContests();
}
