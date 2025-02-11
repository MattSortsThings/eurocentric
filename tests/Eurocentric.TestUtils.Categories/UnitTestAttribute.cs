using Xunit.v3;

namespace Eurocentric.TestUtils.Categories;

/// <summary>
/// Applies the "Unit Test" category.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class UnitTestAttribute : Attribute, ITraitAttribute
{
    public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits() =>
    [
        new KeyValuePair<string, string>("Category", "Unit Test"),
        new KeyValuePair<string, string>("Categorized", "true"),
    ];
}
