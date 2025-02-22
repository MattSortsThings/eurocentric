using Eurocentric.Shared.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Shared.Documentation;

internal sealed class ApiKeySecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider)
    : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        IEnumerable<AuthenticationScheme> schemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (schemes.Any(scheme => scheme.Name == ApiKeyAuthenticationHandler.SchemeName))
        {
            Dictionary<string, OpenApiSecurityScheme> requirements = new()
            {
                [ApiKeyAuthenticationHandler.SchemeName] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = ApiKeyAuthenticationHandler.SchemeName,
                    In = ParameterLocation.Header,
                    Name = "X-Api-Key",
                    BearerFormat = "Api key"
                }
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            foreach (KeyValuePair<OperationType, OpenApiOperation> operation in document.Paths.Values.SelectMany(path =>
                         path.Operations))
            {
                operation.Value.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = ApiKeyAuthenticationHandler.SchemeName, Type = ReferenceType.SecurityScheme } }] =
                        Array.Empty<string>()
                });
            }
        }
    }
}
