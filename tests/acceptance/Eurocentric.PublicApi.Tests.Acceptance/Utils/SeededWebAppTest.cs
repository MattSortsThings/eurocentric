using Eurocentric.Tests.Utils.Attributes;
using Eurocentric.Tests.Utils.Fixtures;

namespace Eurocentric.PublicApi.Tests.Acceptance.Utils;

[DatabaseTest]
public abstract class SeededWebAppTest
{
    private readonly SeededWebAppFixture _fixture;

    protected SeededWebAppTest(SeededWebAppFixture fixture)
    {
        _fixture = fixture;
    }

    private protected ITestableWebApi Sut => _fixture;
}
