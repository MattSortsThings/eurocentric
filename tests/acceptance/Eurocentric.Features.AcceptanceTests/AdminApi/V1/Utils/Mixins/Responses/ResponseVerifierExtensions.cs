using System.Text.Json;

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

    public static void Then_the_response_problem_details_extensions_should_contain(this IResponseVerifier verifier,
        string value = "",
        string key = "")
    {
        Assert.NotNull(verifier.ResponseProblemDetails);

        Assert.Contains(verifier.ResponseProblemDetails.Extensions, kvp => kvp.Key == key
                                                                           && kvp.Value is JsonElement je
                                                                           && je.GetString() == value);
    }

    public static void Then_the_response_problem_details_extensions_should_contain(this IResponseVerifier verifier,
        int value = 0,
        string key = "")
    {
        Assert.NotNull(verifier.ResponseProblemDetails);

        Assert.Contains(verifier.ResponseProblemDetails.Extensions, kvp => kvp.Key == key
                                                                           && kvp.Value is JsonElement je
                                                                           && je.GetInt32() == value);
    }
}
