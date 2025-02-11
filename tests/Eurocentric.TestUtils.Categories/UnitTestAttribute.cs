using Xunit.v3;

namespace Eurocentric.TestUtils.Categories;

/// <summary>
///     Applies the "UnitTest" category.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class UnitTestAttribute : Attribute, ITraitAttribute
{
    public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits() =>
    [
        new("Category", "UnitTest"),
        new("Categorized", "true")
    ];
}
