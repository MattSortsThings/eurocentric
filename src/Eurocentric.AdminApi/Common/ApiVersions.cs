using Asp.Versioning;

namespace Eurocentric.AdminApi.Common;

internal static class ApiVersions
{
    internal static class V0
    {
        internal static readonly ApiVersion Point1 = new(0, 1);
        internal static readonly ApiVersion Point2 = new(0, 2);
    }
}
