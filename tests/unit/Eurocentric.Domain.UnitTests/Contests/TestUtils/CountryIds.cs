using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.Contests.TestUtils;

internal static class CountryIds
{
    internal static readonly CountryId At = CountryId.FromValue(Guid.Parse("aaaaaaaa-1234-5678-8ecf-2d7e2955565b"));
    internal static readonly CountryId Be = CountryId.FromValue(Guid.Parse("bbbbbbbb-1234-5678-8ecf-2d7e2955565b"));
    internal static readonly CountryId Cz = CountryId.FromValue(Guid.Parse("cccccccc-1234-5678-8ecf-2d7e2955565b"));
    internal static readonly CountryId De = CountryId.FromValue(Guid.Parse("dddddddd-1234-5678-8ecf-2d7e2955565b"));
    internal static readonly CountryId Es = CountryId.FromValue(Guid.Parse("eeeeeeee-1234-5678-8ecf-2d7e2955565b"));
    internal static readonly CountryId Fi = CountryId.FromValue(Guid.Parse("ffffffff-1234-5678-8ecf-2d7e2955565b"));
    internal static readonly CountryId Xx = CountryId.FromValue(Guid.Parse("00000000-1234-5678-8ecf-2d7e2955565b"));
}
