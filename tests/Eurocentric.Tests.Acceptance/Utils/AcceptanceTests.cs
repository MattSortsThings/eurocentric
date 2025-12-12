using RestSharp;

namespace Eurocentric.Tests.Acceptance.Utils;

[Category("acceptance")]
[ParallelLimiter<ParallelLimit>]
public abstract class AcceptanceTests
{
    [ClassDataSource<WebApp>(Shared = SharedType.PerClass)]
    public required WebApp SystemUnderTest { get; init; }

    [After(Test)]
    public async ValueTask ResetAsync() => await SystemUnderTest.EraseAllDataAsync();

    private protected static RestRequest GetRequest(string route) => new(route);
}
