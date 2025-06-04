using Eurocentric.Features.AcceptanceTests.Utilities;

namespace Eurocentric.Features.AcceptanceTests.Shared.Utilities;

[CollectionDefinition(nameof(SharedFeaturesTestCollection))]
public sealed class SharedFeaturesTestCollection : ICollectionFixture<WebAppFixture>;
