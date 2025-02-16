using Xunit.v3;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

/// <summary>
///     Applies the "Category=AcceptanceTest" and "Categorized=true" traits.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
internal sealed class AcceptanceTestAttribute : Attribute, ITraitAttribute
{
    public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits() =>
    [
        new("Category", "AcceptanceTest"),
        new("Categorized", "true")
    ];
}
