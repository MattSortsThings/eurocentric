using Eurocentric.TestUtils.WebAppFixtures;

namespace Eurocentric.Features.SubcutaneousTests.Utils;

[CollectionDefinition(nameof(WebAppFixtureTestCollection))]
public sealed class WebAppFixtureTestCollection : ICollectionFixture<WebAppFixture>;
