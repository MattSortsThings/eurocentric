using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.Configuration;

/// <summary>
///     Configures options used for reading and writing JSON when using <see cref="HttpRequestJsonExtensions" /> and
///     <see cref="HttpRequestJsonExtensions" />.
/// </summary>
public sealed class ConfigureHttpJsonOptions : IConfigureOptions<JsonOptions>
{
    /// <inheritdoc />
    public void Configure(JsonOptions options)
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
        options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.SerializerOptions.WriteIndented = false;
    }
}
