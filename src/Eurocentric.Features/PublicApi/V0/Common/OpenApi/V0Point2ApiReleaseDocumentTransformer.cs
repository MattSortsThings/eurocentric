using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V0.Common.OpenApi;

internal sealed class V0Point2ApiReleaseDocumentTransformer : ApiReleaseDocumentTransformer
{
    private protected override string ReleaseUriSegments => "/public/api/v0.2";
}
