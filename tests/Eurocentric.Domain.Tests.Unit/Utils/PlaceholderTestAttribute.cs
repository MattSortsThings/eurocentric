using Xunit.v3;

namespace Eurocentric.Domain.Tests.Unit.Utils;

/// <summary>
///     Applies the "PlaceholderTest" category trait and the "true" categorized trait to annotated tests.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class PlaceholderTestAttribute : Attribute, ITraitAttribute
{
    public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits() =>
    [
        new("Category", "PlaceholderTest"),
        new("Categorized", "true")
    ];
}
