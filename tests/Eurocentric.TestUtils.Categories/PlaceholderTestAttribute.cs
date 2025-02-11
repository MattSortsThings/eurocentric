using Xunit.v3;

namespace Eurocentric.TestUtils.Categories;

/// <summary>
/// Applies the "Placeholder Test" category.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class PlaceholderTestAttribute : Attribute, ITraitAttribute
{
    public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits() =>
    [
        new KeyValuePair<string, string>("Category", "Placeholder Test"),
        new KeyValuePair<string, string>("Categorized", "true"),
    ];
}
