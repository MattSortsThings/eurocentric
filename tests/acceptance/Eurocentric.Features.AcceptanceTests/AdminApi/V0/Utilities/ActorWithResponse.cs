using Eurocentric.Features.AcceptanceTests.Utilities;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utilities;

public abstract class ActorWithResponse<TResponse> : ActorBase
{
    protected ActorWithResponse(IAdminApiV0Driver apiDriver)
    {
        ApiDriver = apiDriver;
        SendMyRequest = _ => throw new InvalidOperationException("SendMyRequest delegate has not been set.");
    }

    private protected IAdminApiV0Driver ApiDriver { get; }

    private protected TResponse? ResponseObject { get; private set; }

    private protected Func<IAdminApiV0Driver, Task<ProblemOrResponse<TResponse>>> SendMyRequest { get; set; }

    public override async Task When_I_send_my_request()
    {
        ProblemOrResponse<TResponse> problemOrResponse = await SendMyRequest(ApiDriver);

        problemOrResponse.Switch(problem =>
        {
            ResponseStatusCode = problem.StatusCode;
            ResponseProblemDetails = problem.Data;
        }, response =>
        {
            ResponseStatusCode = response.StatusCode;
            ResponseObject = response.Data;
        });
    }
}
