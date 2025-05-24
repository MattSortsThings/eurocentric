using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.TestUtils;

public static class CountryIds
{
    public static readonly CountryId At = CountryId.FromValue(Guid.Parse("aaaaaaaa-3e84-40d3-b79e-144326c69592"));
    public static readonly CountryId Be = CountryId.FromValue(Guid.Parse("bbbbbbbb-3e84-40d3-b79e-144326c69592"));
    public static readonly CountryId Cz = CountryId.FromValue(Guid.Parse("cccccccc-3e84-40d3-b79e-144326c69592"));
    public static readonly CountryId De = CountryId.FromValue(Guid.Parse("dddddddd-3e84-40d3-b79e-144326c69592"));
    public static readonly CountryId Ee = CountryId.FromValue(Guid.Parse("eeeeeeee-3e84-40d3-b79e-144326c69592"));
    public static readonly CountryId Fi = CountryId.FromValue(Guid.Parse("ffffffff-3e84-40d3-b79e-144326c69592"));
    public static readonly CountryId It = CountryId.FromValue(Guid.Parse("ffffffff-3e84-ffff-b79e-144326c69592"));
    public static readonly CountryId Xx = CountryId.FromValue(Guid.Parse("00000000-3e84-40d3-b79e-144326c69592"));
}
