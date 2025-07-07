namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

[CollectionDefinition(nameof(AdminApiV0TestCollection), DisableParallelization = false)]
public sealed class AdminApiV0TestCollection : ICollectionFixture<WebAppFixture>;
