using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V0.Common.Contracts;

public sealed record Country(string CountryCode, string CountryName) : IExampleProvider<Country>
{
    public static Country CreateExample() => new("AT", "Austria");
}
