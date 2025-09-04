using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.OpenApi;

internal sealed class V1Point0ApiReleaseDocumentTransformer : ApiReleaseDocumentTransformer
{
    private protected override string ReleaseUriSegments => "/admin/api/v1.0";
}
