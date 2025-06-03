using Eurocentric.Features.AcceptanceTests.Utilities;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utilities;

[CollectionDefinition(nameof(PublicApiV0TestCollection))]
public sealed class PublicApiV0TestCollection : ICollectionFixture<WebAppFixture>;
