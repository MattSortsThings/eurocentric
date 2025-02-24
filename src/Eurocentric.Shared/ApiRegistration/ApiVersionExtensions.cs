using Asp.Versioning;

namespace Eurocentric.Shared.ApiRegistration;

internal static class ApiVersionExtensions
{
    internal static bool ShouldIncludeIn(this ApiVersion subject, ApiVersion comparand) =>
        subject.MajorVersion == comparand.MajorVersion && subject.MinorVersion <= comparand.MinorVersion;
}
