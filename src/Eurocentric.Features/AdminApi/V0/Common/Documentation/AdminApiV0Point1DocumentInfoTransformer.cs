using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V0.Common.Documentation;

internal sealed class AdminApiV0Point1DocumentInfoTransformer : DocumentInfoTransformer
{
    private protected override string Title => "Eurocentric Admin API";

    private protected override string Description => "A web API for modelling the Eurovision Song Contest, 2016-present.";

    private protected override string Version => "v0.1";
}
