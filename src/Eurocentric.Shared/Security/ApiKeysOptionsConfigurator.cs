using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Eurocentric.Shared.Security;

internal sealed class ApiKeysOptionsConfigurator(IConfiguration configuration) : IConfigureOptions<ApiKeysOptions>
{
    private const string SectionName = "ApiKeys";

    public void Configure(ApiKeysOptions options) => configuration.GetSection(SectionName).Bind(options);
}
