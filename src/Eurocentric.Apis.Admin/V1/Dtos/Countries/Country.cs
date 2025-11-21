using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Countries;

/// <summary>
///     Represents a country or pseudo-country.
/// </summary>
public sealed record Country : IDtoSchemaExampleProvider<Country>
{
    /// <summary>
    ///     The country's ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     The country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     An array of all the country's contest roles.
    /// </summary>
    public ContestRole[] ContestRoles { get; init; } = [];

    public static Country CreateExample() =>
        new()
        {
            Id = V1ExampleIds.CountryA,
            CountryCode = "AT",
            CountryName = "Austria",
            ContestRoles = [ContestRole.CreateExample()],
        };
}
