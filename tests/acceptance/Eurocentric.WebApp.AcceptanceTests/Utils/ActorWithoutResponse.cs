using System.Net;

namespace Eurocentric.WebApp.AcceptanceTests.Utils;

public abstract class ActorWithoutResponse : Actor
{
    public async Task Then_my_request_should_SUCCEED_with_status_code_204_NoContent() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.NoContent);
}
