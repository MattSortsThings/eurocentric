using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.UnitTests.Domain.Aggregates.TestUtils;

public static class CountryIds
{
    public static readonly CountryId At = CountryId.FromValue(Guid.Parse("01abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Be = CountryId.FromValue(Guid.Parse("02abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Cz = CountryId.FromValue(Guid.Parse("03abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Dk = CountryId.FromValue(Guid.Parse("04abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Ee = CountryId.FromValue(Guid.Parse("05abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Fi = CountryId.FromValue(Guid.Parse("06abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Gb = CountryId.FromValue(Guid.Parse("07abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Hr = CountryId.FromValue(Guid.Parse("08abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId It = CountryId.FromValue(Guid.Parse("09abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Lv = CountryId.FromValue(Guid.Parse("12abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Mt = CountryId.FromValue(Guid.Parse("13abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId No = CountryId.FromValue(Guid.Parse("14abcdef-eaa6-4c18-89ed-ea537b06b092"));
    public static readonly CountryId Xx = CountryId.FromValue(Guid.Parse("24abcdef-eaa6-4c18-89ed-ea537b06b092"));

    public static IEnumerable<CountryId> Generate(int count)
    {
        Random random = new(999);

        for (int i = 0; i < count; i++)
        {
            byte[] bytes = new byte[16];
            random.NextBytes(bytes);

            yield return CountryId.FromValue(new Guid(bytes));
        }
    }
}
