using Eurocentric.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.Tests.Acceptance;

[Category("placeholder")]
public sealed class PublicApiPlaceholderTests : AcceptanceTests
{
    [Test]
    [Repeat(5)]
    public async Task Should_be_able_to_ping_Public_API()
    {
        // Arrange
        await using TestWebApp testWebApp = await TestWebApp.ConnectingTo(DbServer).InitializeAsync();

        RestRequest request = new("/public/api/ping");

        // Act
        RestResponse response = await testWebApp.SendAsync(request);

        // Assert
        await Assert.That(response.IsSuccessful).IsTrue();
        await Assert.That(response.Content).IsEqualTo("\"Public API zapped to the extreme!\"");
    }
}
