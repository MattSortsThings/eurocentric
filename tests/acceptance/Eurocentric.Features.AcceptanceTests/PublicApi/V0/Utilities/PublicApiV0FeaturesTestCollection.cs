using Eurocentric.Features.AcceptanceTests.Utilities;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utilities;

[CollectionDefinition(nameof(PublicApiV0FeaturesTestCollection))]
public sealed class PublicApiV0FeaturesTestCollection : ICollectionFixture<WebAppFixture>;
