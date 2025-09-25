using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Eurocentric.Infrastructure.DataAccess.Common;

public sealed class AzureSqlDbOptionsConfig(IConfiguration configuration)
    : IConfigureOptions<AzureSqlDbOptions>
{
    public void Configure(AzureSqlDbOptions options) =>
        configuration.GetSection("AzureSqlDb").Bind(options);
}
