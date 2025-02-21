using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Eurocentric.Shared.Json;

internal sealed class JsonOptionsConfiguration : IConfigureOptions<JsonOptions>
{
    public void Configure(JsonOptions options)
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    }
}
