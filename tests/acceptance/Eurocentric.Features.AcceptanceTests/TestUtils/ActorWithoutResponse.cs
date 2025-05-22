namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public abstract class ActorWithoutResponse : ActorBase
{
    private Func<Task<ResponseOrProblem>> SendMyRequest { get; set; } =
        () => throw new InvalidOperationException("SendMyRequest function not set.");

    public async Task When_I_send_my_request()
    {
        ResponseOrProblem responseOrProblem = await SendMyRequest();

        responseOrProblem.Switch(success =>
        {
            StatusCode = success.StatusCode;
        }, problem =>
        {
            StatusCode = problem.StatusCode;
            ProblemDetails = problem.Data;
        });
    }
}
