using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Scalar.AspNetCore;

namespace Eurocentric.Features.Shared.Documentation;

internal static class Configuration
{
    internal static void UseDocumentationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapOpenApi().AllowAnonymous();

        app.MapScalarApiReference("docs").AllowAnonymous();
    }
}
