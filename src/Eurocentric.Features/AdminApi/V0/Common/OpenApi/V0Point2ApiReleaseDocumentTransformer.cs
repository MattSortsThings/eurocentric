using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V0.Common.OpenApi;

internal sealed class V0Point2ApiReleaseDocumentTransformer : ApiReleaseDocumentTransformer
{
    private protected override string ReleaseUriSegments => "/admin/api/v0.2";
}
