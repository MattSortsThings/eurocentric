namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Responses;

public static class ResponseVerifierExtensions
{
    public static void Then_my_request_should_fail_with_status_code(this IResponseVerifier verifier, int statusCode)
    {
        int actualStatusCode = (int)verifier.ResponseStatusCode;

        Assert.InRange(actualStatusCode, 400, 499);
        Assert.Equal(statusCode, actualStatusCode);
    }

    public static void Then_my_request_should_succeed_with_status_code(this IResponseVerifier verifier, int statusCode)
    {
        int actualStatusCode = (int)verifier.ResponseStatusCode;

        Assert.InRange(actualStatusCode, 200, 299);
        Assert.Equal(statusCode, actualStatusCode);
    }

    public static void Then_the_response_problem_details_should_match(this IResponseVerifier verifier,
        string detail = "",
        string title = "",
        int status = 0)
    {
        Assert.NotNull(verifier.ResponseProblemDetails);

        Assert.Equal(status, verifier.ResponseProblemDetails.Status);
        Assert.Equal(title, verifier.ResponseProblemDetails.Title);
        Assert.Equal(detail, verifier.ResponseProblemDetails.Detail);
    }
}
