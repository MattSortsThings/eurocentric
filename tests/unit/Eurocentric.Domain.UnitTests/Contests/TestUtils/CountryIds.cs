using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.Contests.TestUtils;

internal static class CountryIds
{
    internal static readonly CountryId At = CountryId.FromValue(Guid.Parse("00000000-1234-5678-8ecf-2d7e2955565b"));
    internal static readonly CountryId Be = CountryId.FromValue(Guid.Parse("11111111-1234-5678-8ecf-2d7e2955565b"));
    internal static readonly CountryId Cz = CountryId.FromValue(Guid.Parse("22222222-1234-5678-8ecf-2d7e2955565b"));
    internal static readonly CountryId De = CountryId.FromValue(Guid.Parse("33333333-1234-5678-8ecf-2d7e2955565b"));
    internal static readonly CountryId Es = CountryId.FromValue(Guid.Parse("44444444-1234-5678-8ecf-2d7e2955565b"));
    internal static readonly CountryId Fi = CountryId.FromValue(Guid.Parse("55555555-1234-5678-8ecf-2d7e2955565b"));
    internal static readonly CountryId Gb = CountryId.FromValue(Guid.Parse("66666666-1234-5678-8ecf-2d7e2955565b"));
    internal static readonly CountryId Xx = CountryId.FromValue(Guid.Parse("77777777-1234-5678-8ecf-2d7e2955565b"));
}
