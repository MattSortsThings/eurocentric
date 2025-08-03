using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V0.Common.OpenApi;

internal sealed class InfoDocumentTransformer(string apiVersion) : BaseInfoDocumentTransformer
{
    private protected override string Title => "Eurocentric Public API";

    private protected override string Description => "A web API for (over)analysing the Eurovision Song Contest, 2016-2025.";

    private protected override string Version { get; } = apiVersion;
}
