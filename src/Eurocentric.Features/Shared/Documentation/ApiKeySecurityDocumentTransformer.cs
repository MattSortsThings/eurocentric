using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Features.Shared.Documentation;

internal sealed class ApiKeySecurityDocumentTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider)
    : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        IEnumerable<AuthenticationScheme> schemes = await authenticationSchemeProvider.GetAllSchemesAsync();

        if (schemes.Any(scheme => scheme.Name == ApiKeyAuthenticationScheme.SchemeName))
        {
            Dictionary<string, OpenApiSecurityScheme> requirements = new()
            {
                [ApiKeyAuthenticationScheme.SchemeName] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = ApiKeyAuthenticationScheme.SchemeName,
                    In = ParameterLocation.Header,
                    Name = ApiKeyAuthenticationScheme.HttpRequestHeaderName,
                    BearerFormat = "Api key"
                }
            };

            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            foreach (KeyValuePair<OperationType, OpenApiOperation> operation in document.Paths.Values.SelectMany(path =>
                         path.Operations))
            {
                OpenApiReference openApiReference = new()
                {
                    Id = ApiKeyAuthenticationScheme.SchemeName, Type = ReferenceType.SecurityScheme
                };

                OpenApiSecurityScheme scheme = new() { Reference = openApiReference };

                operation.Value.Security.Add(new OpenApiSecurityRequirement { [scheme] = Array.Empty<string>() });
            }
        }
    }
}
