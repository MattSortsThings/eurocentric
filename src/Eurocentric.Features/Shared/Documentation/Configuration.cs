using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.Shared.Documentation;

internal static class Configuration
{
    internal static void UseDocumentationEndpoints(this IEndpointRouteBuilder app) => app.MapOpenApi().AllowAnonymous();
}
