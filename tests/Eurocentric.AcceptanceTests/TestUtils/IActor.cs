using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.AcceptanceTests.TestUtils;

public interface IActor
{
    /// <summary>
    ///     The REST request that is sent to the web app fixture during the "When" phase.
    /// </summary>
    RestRequest? Request { get; }

    /// <summary>
    ///     A successful REST response that is received from the web app fixture during the "When" phase.
    /// </summary>
    RestResponse? SuccessResponse { get; }

    /// <summary>
    ///     An unsuccessful REST response that is received from the web app fixture during the "When" phase.
    /// </summary>
    RestResponse<ProblemDetails>? FailureResponse { get; }

    Task When_I_send_my_request();

    Task Then_my_request_should_SUCCEED_with_status_code(int statusCode);

    Task Then_my_request_should_FAIL_with_status_code(int statusCode);

    Task Then_the_response_problem_details_should_match(string detail = "", string title = "", int status = 0);
}
