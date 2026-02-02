using Humanizer;

namespace Eurocentric.Tests.Unit.Utils;

public sealed class QualifiedDisplayNameAttribute : DisplayNameFormatterAttribute
{
    protected override string FormatDisplayName(DiscoveredTestContext context) =>
        FormatNestedTestClassNames(context) + " - " + FormatTestMethodAndParameters(context);

    private static string FormatNestedTestClassNames(DiscoveredTestContext context)
    {
        Type classType = context.TestContext.ClassContext.ClassType;

        string? outerName = classType.DeclaringType?.Name.Replace("Tests", string.Empty);

        return outerName is null ? $"{classType.Name} - " : $"{outerName} - {classType.Name}";
    }

    private static string FormatTestMethodAndParameters(DiscoveredTestContext context)
    {
        string displayName = context.GetDisplayName();

        return displayName.IndexOf('(') is var openParenIndex and > 0
            ? $"{displayName[..openParenIndex].Humanize()} {displayName[openParenIndex..]}"
            : displayName.Humanize();
    }
}
