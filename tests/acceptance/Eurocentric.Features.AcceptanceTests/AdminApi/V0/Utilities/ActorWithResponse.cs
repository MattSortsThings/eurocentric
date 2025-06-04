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

    private protected Func<IAdminApiV0Driver, Task<ResponseOrProblem<TResponse>>> SendMyRequest { get; set; }

    public override async Task When_I_send_my_request()
    {
        ResponseOrProblem<TResponse> responseOrProblem = await SendMyRequest(ApiDriver);

        responseOrProblem.Switch(response =>
        {
            ResponseStatusCode = response.StatusCode;
            ResponseObject = response.Data;
        }, problem =>
        {
            ResponseStatusCode = problem.StatusCode;
            ResponseProblemDetails = problem.Data;
        });
    }
}
