using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.Aggregates.Utils;

/// <summary>
///     A set of eight named <see cref="CountryId" /> values whose sort order matches their property names.
/// </summary>
public sealed record EightCountryIds
{
    public CountryId At { get; } = CountryId.FromValue(Guid.Parse("01acb390-f9c6-4f7f-b0e5-d8cb87500d8e"));

    public CountryId Be { get; } = CountryId.FromValue(Guid.Parse("02acb390-f9c6-4f7f-b0e5-d8cb87500d8e"));

    public CountryId Cz { get; } = CountryId.FromValue(Guid.Parse("03acb390-f9c6-4f7f-b0e5-d8cb87500d8e"));

    public CountryId Dk { get; } = CountryId.FromValue(Guid.Parse("04acb390-f9c6-4f7f-b0e5-d8cb87500d8e"));

    public CountryId Ee { get; } = CountryId.FromValue(Guid.Parse("05acb390-f9c6-4f7f-b0e5-d8cb87500d8e"));

    public CountryId Fi { get; } = CountryId.FromValue(Guid.Parse("06acb390-f9c6-4f7f-b0e5-d8cb87500d8e"));

    public CountryId Gb { get; } = CountryId.FromValue(Guid.Parse("07acb390-f9c6-4f7f-b0e5-d8cb87500d8e"));

    public CountryId Hu { get; } = CountryId.FromValue(Guid.Parse("08acb390-f9c6-4f7f-b0e5-d8cb87500d8e"));

    public void Deconstruct(out CountryId at, out CountryId be, out CountryId cz, out CountryId dk, out CountryId ee,
        out CountryId fi, out CountryId gb, out CountryId hu)
    {
        at = At;
        be = Be;
        cz = Cz;
        dk = Dk;
        ee = Ee;
        fi = Fi;
        gb = Gb;
        hu = Hu;
    }
}
