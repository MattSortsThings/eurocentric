using Eurocentric.TestUtils.WebAppFixtures;

namespace Eurocentric.Features.AcceptanceTests.Utils;

[CollectionDefinition(nameof(WebAppFixtureTestCollection))]
public sealed class WebAppFixtureTestCollection : ICollectionFixture<WebAppFixture>;
