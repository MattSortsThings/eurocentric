using Eurocentric.Apis.Admin.UnitTests.Utils;
using Eurocentric.Apis.Admin.V0.Enums;

namespace Eurocentric.Apis.Admin.UnitTests;

[Category("placeholder")]
public sealed class AdminApiPlaceholderTests : UnitTest
{
    [Test]
    public async Task Should_always_pass()
    {
        CountryType[] values = Enum.GetValues<CountryType>();

        await Assert.That(values).HasCount(2);
    }
}
