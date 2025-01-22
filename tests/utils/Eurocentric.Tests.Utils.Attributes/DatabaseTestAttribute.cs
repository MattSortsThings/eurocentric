using Xunit.v3;

namespace Eurocentric.Tests.Utils.Attributes;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class DatabaseTestAttribute : Attribute, ITraitAttribute
{
    public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits() =>
    [
        new("Category", "DatabaseTest"),
        new("Categorized", "true")
    ];
}
