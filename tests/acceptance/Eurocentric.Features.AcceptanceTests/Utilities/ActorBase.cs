using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.AcceptanceTests.Utilities;

public abstract class ActorBase
{
    private protected HttpStatusCode ResponseStatusCode { get; set; }

    private protected ProblemDetails? ResponseProblemDetails { get; set; }

    public abstract Task When_I_send_my_request();

    public void Then_my_request_should_succeed_with_status_code_200_OK() =>
        Assert.Equal(HttpStatusCode.OK, ResponseStatusCode);

    public void Then_my_request_should_succeed_with_status_code_201_Created() =>
        Assert.Equal(HttpStatusCode.Created, ResponseStatusCode);

    public void Then_my_request_should_succeed_with_status_code_204_NoContent() =>
        Assert.Equal(HttpStatusCode.NoContent, ResponseStatusCode);

    public void Then_my_request_should_fail_with_status_code_404_NotFound() =>
        Assert.Equal(HttpStatusCode.NotFound, ResponseStatusCode);

    public void Then_my_request_should_fail_with_status_code_409_Conflict() =>
        Assert.Equal(HttpStatusCode.Conflict, ResponseStatusCode);

    public void Then_my_request_should_fail_with_status_code_422_UnprocessableEntity() =>
        Assert.Equal(HttpStatusCode.UnprocessableEntity, ResponseStatusCode);

    public void Then_the_problem_details_should_match(string title = "", string detail = "", int status = 0)
    {
        Assert.NotNull(ResponseProblemDetails);

        Assert.Equal(title, ResponseProblemDetails.Title);
        Assert.Equal(detail, ResponseProblemDetails.Detail);
        Assert.Equal(status, ResponseProblemDetails.Status);
    }

    protected void Then_the_problem_details_extensions_should_contain(string key, Guid value)
    {
        Assert.NotNull(ResponseProblemDetails);

        Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp.Key == key
                                                                  && kvp.Value is JsonElement e
                                                                  && e.GetGuid() == value);
    }

    public void Then_the_problem_details_extensions_should_contain(string key, string value)
    {
        Assert.NotNull(ResponseProblemDetails);

        Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp.Key == key
                                                                  && kvp.Value is JsonElement e
                                                                  && e.GetString() == value);
    }

    public void Then_the_problem_details_extensions_should_contain(string key, int value)
    {
        Assert.NotNull(ResponseProblemDetails);

        Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp.Key == key
                                                                  && kvp.Value is JsonElement e
                                                                  && e.GetInt32() == value);
    }
}
