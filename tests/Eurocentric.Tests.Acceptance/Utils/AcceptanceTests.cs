using RestSharp;

namespace Eurocentric.Tests.Acceptance.Utils;

[Category("acceptance")]
[ParallelLimiter<ParallelLimit>]
public abstract class AcceptanceTests
{
    [ClassDataSource<WebAppFixture>(Shared = SharedType.PerClass)]
    public required IWebAppFixture SystemUnderTest { get; init; }

    [After(Test)]
    public async ValueTask ResetAsync() => await SystemUnderTest.EraseAllDataAsync();

    private protected static RestRequest GetRequest(string route) => new(route);
}
