using Asp.Versioning;

namespace Eurocentric.PublicApi.Common;

internal static class ApiVersions
{
    internal static class V0
    {
        internal static readonly ApiVersion Point1 = new(0, 1);
    }
}
