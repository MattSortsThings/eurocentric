using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.UnitTests.Domain.Aggregates.Contests.TestUtils;

public static class CountryIds
{
    public static readonly CountryId At = CountryId.FromValue(Guid.Parse("01abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Be = CountryId.FromValue(Guid.Parse("02abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Cz = CountryId.FromValue(Guid.Parse("03abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Dk = CountryId.FromValue(Guid.Parse("04abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Ee = CountryId.FromValue(Guid.Parse("05abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Fi = CountryId.FromValue(Guid.Parse("06abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Xx = CountryId.FromValue(Guid.Parse("24abcdef-eaa6-4c18-89ed-ea537b06b092"));
}
