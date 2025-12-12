using Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;
using Eurocentric.Tests.Acceptance.Utils;

namespace Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Countries;

[Category("admin-api")]
[NotInParallel("AdminApi.V0.Countries.CreateCountryTests")]
public sealed partial class CreateCountryTests : AcceptanceTests
{
    private abstract class FeatureActor : ActorWithResponse<CreateCountryResponse>
    {
        protected FeatureActor(IActorKernel kernel)
            : base(kernel) { }
    }
}
