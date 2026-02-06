using Eurocentric.Tests.Acceptance.Utils;

namespace Eurocentric.Tests.Acceptance.AdminApi.V0.Utils;

[Category("admin-api")]
[NotInParallel(nameof(AdminApiV0AcceptanceTests))]
public abstract class AdminApiV0AcceptanceTests : AcceptanceTests;
