using System.Net;

namespace Eurocentric.WebApp.AcceptanceTests.Utils;

public abstract class ActorWithResponse<TResponse> : Actor where TResponse : class
{
    public TResponse? ResponseObject { get; private protected set; }

    public async Task Then_my_request_should_SUCCEED_with_status_code_200_OK() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.OK);

    public async Task Then_my_request_should_SUCCEED_with_status_code_201_Created() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.Created);
}
