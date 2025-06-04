using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Features.Shared.Documentation;

/// <summary>
///     Adds API key security scheme to OpenAPI documents.
/// </summary>
/// <remarks>
///     This class has been adapted from the example
///     <a
///         href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/customize-openapi?view=aspnetcore-9.0#use-document-transformers">
///         BearerSecuritySchemeTransformer
///     </a>
///     class in Microsoft's ASP.NET documentation.
/// </remarks>
/// <param name="authenticationSchemeProvider">Provides authentication schemes for the web application.</param>
internal sealed class ApiKeySecurityDocumentTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider)
    : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        IEnumerable<AuthenticationScheme> authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

        if (authenticationSchemes.Any(authScheme => authScheme.Name == ApiKeyAuthenticationScheme.SchemeName))
        {
            Dictionary<string, OpenApiSecurityScheme> requirements = new()
            {
                [ApiKeyAuthenticationScheme.SchemeName] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = ApiKeyAuthenticationScheme.SchemeName,
                    In = ParameterLocation.Header,
                    BearerFormat = "API key",
                    Name = ApiKeyAuthenticationScheme.HttpRequestHeaderName
                }
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;
        }
    }
}
