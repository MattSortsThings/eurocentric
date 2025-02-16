using Xunit.v3;

namespace Eurocentric.PublicApi.Tests.Integration.Utils;

/// <summary>
///     Applies the "Category=IntegrationTest" and "Categorized=true" traits.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
internal sealed class IntegrationTestAttribute : Attribute, ITraitAttribute
{
    public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits() =>
    [
        new("Category", "IntegrationTest"),
        new("Categorized", "true")
    ];
}
