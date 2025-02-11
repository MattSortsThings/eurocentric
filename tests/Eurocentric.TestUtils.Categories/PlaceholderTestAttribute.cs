using Xunit.v3;

namespace Eurocentric.TestUtils.Categories;

/// <summary>
///     Applies the "PlaceholderTest" category.
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
