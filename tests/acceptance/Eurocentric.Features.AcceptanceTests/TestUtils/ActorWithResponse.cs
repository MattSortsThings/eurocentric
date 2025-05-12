namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public abstract class ActorWithResponse<TResponse> : ActorBase
    where TResponse : class
{
    private protected TResponse? Response { get; private set; }

    private protected abstract Func<Task<ResponseOrProblem<TResponse>>> SendMyRequest { get; set; }

    public async Task When_I_send_my_request()
    {
        ResponseOrProblem<TResponse> responseOrProblem = await SendMyRequest();

        responseOrProblem.Switch(success =>
        {
            StatusCode = success.StatusCode;
            Response = success.Data;
        }, problem =>
        {
            StatusCode = problem.StatusCode;
            ProblemDetails = problem.Data;
        });
    }
}
