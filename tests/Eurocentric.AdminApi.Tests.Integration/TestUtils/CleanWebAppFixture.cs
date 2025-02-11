using Eurocentric.TestUtils.WebAppFixtures;

namespace Eurocentric.AdminApi.Tests.Integration.TestUtils;

public sealed class CleanWebAppFixture : WebAppFixture
{
    public void Reset() => ResetDatabase();
}
