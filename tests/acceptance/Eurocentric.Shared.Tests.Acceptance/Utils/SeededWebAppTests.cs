using Eurocentric.Tests.Utils.Attributes;
using Eurocentric.Tests.Utils.Fixtures;

namespace Eurocentric.Shared.Tests.Acceptance.Utils;

[DatabaseTest]
public abstract class SeededWebAppTests
{
    private protected const string AdminApiPrefix = "admin/api/v0.1/";
    private readonly SeededWebAppFixture _fixture;

    protected SeededWebAppTests(SeededWebAppFixture fixture)
    {
        _fixture = fixture;
    }

    private protected ITestableWebApi Sut => _fixture;
}
