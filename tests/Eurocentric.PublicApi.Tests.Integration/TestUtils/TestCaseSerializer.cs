using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit.Sdk;

namespace Eurocentric.PublicApi.Tests.Integration.TestUtils;

public sealed class TestCaseSerializer : IXunitSerializer
{
    private readonly JsonSerializerOptions _options = new() { Converters = { new JsonStringEnumConverter() } };

    public object Deserialize(Type type, string serializedValue) =>
        JsonSerializer.Deserialize(serializedValue, type, _options)!;

    public bool IsSerializable(Type type, object? value, [NotNullWhen(false)] out string? failureReason)
    {
        failureReason = null;

        return true;
    }

    public string Serialize(object value) => JsonSerializer.Serialize(value, _options);
}
