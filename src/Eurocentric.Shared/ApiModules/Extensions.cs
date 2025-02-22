using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Eurocentric.Shared.ApiModules;

internal static class Extensions
{
    internal static RouteHandlerBuilder WithApiVersions(this RouteHandlerBuilder builder, IEnumerable<ApiVersion> apiVersions)
    {
        foreach (ApiVersion version in apiVersions)
        {
            builder.HasApiVersion(version);
        }

        return builder;
    }

    internal static bool IsIncludedIn(this ApiVersion subject, ApiVersion comparand) =>
        subject.MajorVersion == comparand.MajorVersion && subject.MinorVersion <= comparand.MinorVersion;

    internal static bool Matches(this ApiDescription description, string groupName, ApiVersion apiVersion) =>
        description.GroupName == groupName && description.GetApiVersion() is { } version && version == apiVersion;
}
