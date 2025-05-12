using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V0.Common.Documentation;

internal sealed class V0Point1DocumentInfoTransformer : DocumentInfoTransformer
{
    private protected override string Title => "Eurocentric Public API";

    private protected override string Description => "A web API for (over)analysing the Eurovision Song Contest, 2016-present.";

    private protected override string Version => "v0.1";
}
