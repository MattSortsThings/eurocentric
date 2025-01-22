using Xunit.v3;

namespace Eurocentric.Tests.Utils.Attributes;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class SubcutaneousTestAttribute : Attribute, ITraitAttribute
{
    public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits() =>
    [
        new("Category", "SubcutaneousTest"),
        new("Categorized", "true")
    ];
}
