using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V0.Common.OpenApi;

internal sealed class V0Point1ApiReleaseDocumentTransformer : ApiReleaseDocumentTransformer
{
    private protected override string ReleaseUriSegments => "/admin/api/v0.1";
}
