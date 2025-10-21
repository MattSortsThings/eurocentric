using Eurocentric.Components.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Eurocentric.Components.OpenApi;

/// <summary>
///     Adds API key security scheme to OpenAPI documents.
/// </summary>
/// <remarks>
///     This class has been adapted from the example
///     <a
///         href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/customize-openapi?view=aspnetcore-10.0">
///         BearerSecuritySchemeTransformer
///     </a>
///     class in Microsoft's ASP.NET Core documentation.
/// </remarks>
/// <param name="provider">Provides authentication schemes for the web application.</param>
public sealed class ApiKeySecurityTransformer(IAuthenticationSchemeProvider provider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        IEnumerable<AuthenticationScheme> authenticationSchemes = await provider.GetAllSchemesAsync();

        if (authenticationSchemes.Any(authScheme => authScheme.Name == AuthenticationConstants.SchemeName))
        {
            Dictionary<string, IOpenApiSecurityScheme> requirements = new()
            {
                [AuthenticationConstants.SchemeName] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = AuthenticationConstants.SchemeName,
                    In = ParameterLocation.Header,
                    BearerFormat = "API key",
                    Name = AuthenticationConstants.HttpRequestHeaderKey,
                },
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;
        }
    }
}
