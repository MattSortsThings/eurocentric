using Asp.Versioning;

namespace Eurocentric.Shared.ApiModules;

internal static class ApiVersionExtensions
{
    internal static string MapToApiReleaseGroupName(this ApiVersion version, string apiName) =>
        $"{apiName}-v{version.MajorVersion}.{version.MinorVersion}";
}
