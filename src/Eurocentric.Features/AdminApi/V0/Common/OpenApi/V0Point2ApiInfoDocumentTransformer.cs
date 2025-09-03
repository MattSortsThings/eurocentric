using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V0.Common.OpenApi;

internal sealed class V0Point2ApiInfoDocumentTransformer : ApiInfoDocumentTransformer
{
    private protected override string Title => "Eurocentric Admin API";

    private protected override string Description => "A web API for modelling the Eurovision Song Contest, 2016-2025.";

    private protected override string ApiVersion => "v0.2";
}
