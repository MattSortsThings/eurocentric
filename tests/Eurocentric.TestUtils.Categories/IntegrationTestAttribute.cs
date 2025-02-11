using Xunit.v3;

namespace Eurocentric.TestUtils.Categories;

/// <summary>
///     Applies the "IntegrationTest" category.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class IntegrationTestAttribute : Attribute, ITraitAttribute
{
    public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits() =>
    [
        new("Category", "IntegrationTest"),
        new("Categorized", "true")
    ];
}
