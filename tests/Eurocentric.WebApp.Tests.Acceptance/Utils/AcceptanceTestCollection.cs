using Eurocentric.TestUtils.WebAppFixtures;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

[CollectionDefinition(nameof(AcceptanceTestCollection))]
public class AcceptanceTestCollection : ICollectionFixture<WebAppFixture>;
