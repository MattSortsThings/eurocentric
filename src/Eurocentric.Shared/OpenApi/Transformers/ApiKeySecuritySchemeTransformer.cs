using Eurocentric.Shared.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Shared.OpenApi.Transformers;

public class ApiKeySecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider)
    : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        IEnumerable<AuthenticationScheme> authSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (authSchemes.Any(scheme => scheme.Name == ApiKeyAuthenticator.SchemeName))
        {
            Dictionary<string, OpenApiSecurityScheme> requirements = new()
            {
                [ApiKeyAuthenticator.SchemeName] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = ApiKeyAuthenticator.SchemeName,
                    In = ParameterLocation.Header,
                    Name = ApiKeyAuthenticator.ApiKeyRequestHeaderName
                }
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;
        }
    }
}
