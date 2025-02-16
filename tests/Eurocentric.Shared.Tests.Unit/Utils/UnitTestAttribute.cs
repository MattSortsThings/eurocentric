using Xunit.v3;

namespace Eurocentric.Shared.Tests.Unit.Utils;

/// <summary>
///     Applies the "UnitTest" category trait and the "true" categorized trait to annotated tests.
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
