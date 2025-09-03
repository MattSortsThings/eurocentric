using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V0.Common.OpenApi;

internal sealed class V0Point1ApiInfoDocumentTransformer : ApiInfoDocumentTransformer
{
    private protected override string Title => "Eurocentric Public API";

    private protected override string Description => "A web API for (over)analysing the Eurovision Song Contest, 2016-2025.";

    private protected override string ApiVersion => "v0.1";
}
