using Humanizer;

namespace Eurocentric.AcceptanceTests.TestUtils;

public sealed class UseQualifiedDisplayNameAttribute : DisplayNameFormatterAttribute
{
    protected override string FormatDisplayName(DiscoveredTestContext context) =>
        GetPrefix(context) + GetSuffix(context);

    private static string GetPrefix(DiscoveredTestContext context)
    {
        Type classType = context.TestContext.ClassContext.ClassType;

        string? outerName = classType.DeclaringType?.Name.Replace("Tests", string.Empty);
        string innerName = classType.Name.Humanize();

        return outerName is null ? $"{innerName} - " : $"{outerName} - {innerName} - ";
    }

    private static string GetSuffix(DiscoveredTestContext context)
    {
        string displayName = context.GetDisplayName();

        return displayName.IndexOf('(') is var openParenIndex and > 0
            ? $"{displayName[..openParenIndex].Humanize()} {displayName[openParenIndex..]}"
            : displayName.Humanize();
    }
}
