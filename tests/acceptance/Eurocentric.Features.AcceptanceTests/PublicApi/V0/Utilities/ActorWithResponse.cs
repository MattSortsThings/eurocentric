using Eurocentric.Features.AcceptanceTests.Utilities;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utilities;

public class ActorWithResponse<TResponse> : ActorBase
{
    public ActorWithResponse(IPublicApiV0Driver apiDriver)
    {
        ApiDriver = apiDriver;
        Request = _ => throw new InvalidOperationException("Request has not been set.");
    }

    private protected IPublicApiV0Driver ApiDriver { get; }

    private protected TResponse? ResponseObject { get; set; }

    private protected Func<IPublicApiV0Driver, Task<ResponseOrProblem<TResponse>>> Request { get; set; }

    public override async Task When_I_send_my_request()
    {
        ResponseOrProblem<TResponse> responseOrProblem = await Request.Invoke(ApiDriver);

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
