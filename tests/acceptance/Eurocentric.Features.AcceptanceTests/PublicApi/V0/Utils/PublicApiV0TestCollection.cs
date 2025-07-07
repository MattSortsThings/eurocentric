namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

[CollectionDefinition(nameof(PublicApiV0TestCollection), DisableParallelization = false)]
public sealed class PublicApiV0TestCollection : ICollectionFixture<WebAppFixture>;
