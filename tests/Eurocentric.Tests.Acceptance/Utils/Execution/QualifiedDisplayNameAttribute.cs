using Humanizer;
using Type = System.Type;

namespace Eurocentric.Tests.Acceptance.Utils.Execution;

public sealed class QualifiedDisplayNameAttribute : DisplayNameFormatterAttribute
{
    protected override string FormatDisplayName(DiscoveredTestContext context) =>
        FormatNestedTestClassNames(context) + " - " + FormatTestMethodAndParameters(context);

    private static string FormatNestedTestClassNames(DiscoveredTestContext context)
    {
        Type classType = context.TestContext.ClassContext.ClassType;

        return string.Join(
            " - ",
            classType
                .Name.Split('_')
                .Select(item => item.Replace("Tests", string.Empty).Humanize().Replace(" api ", " API "))
        );
    }

    private static string FormatTestMethodAndParameters(DiscoveredTestContext context)
    {
        string displayName = context.GetDisplayName();

        return displayName.IndexOf('(') is var openParenIndex and > 0
            ? $"{displayName[..openParenIndex].Humanize()} {displayName[openParenIndex..]}"
            : displayName.Humanize();
    }
}
