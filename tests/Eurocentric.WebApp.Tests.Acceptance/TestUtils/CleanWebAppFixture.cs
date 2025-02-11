using Eurocentric.TestUtils.WebAppFixtures;

namespace Eurocentric.WebApp.Tests.Acceptance.TestUtils;

public sealed class CleanWebAppFixture : WebAppFixture
{
    public void Reset() => ResetDatabase();
}
