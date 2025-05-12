using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Documentation;

internal sealed class V1Point0DocumentInfoTransformer : DocumentInfoTransformer
{
    private protected override string Title => "Eurocentric Admin API";

    private protected override string Description => "A web API for modelling the Eurovision Song Contest, 2016-present.";

    private protected override string Version => "v1.0";
}
