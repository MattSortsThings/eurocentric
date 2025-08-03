using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableCountries;

public sealed record QueryableCountry : IExampleProvider<QueryableCountry>
{
    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public static QueryableCountry CreateExample() => new() { CountryCode = "AT", CountryName = "Austria" };
}
