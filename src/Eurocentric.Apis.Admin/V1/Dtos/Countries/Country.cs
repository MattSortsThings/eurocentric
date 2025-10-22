using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Countries;

/// <summary>
///     Represents a country or pseudo-country.
/// </summary>
public sealed record Country : ISchemaExampleProvider<Country>
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
    public V0.Dtos.Countries.ContestRole[] ContestRoles { get; init; } = [];

    public static Country CreateExample() =>
        new()
        {
            Id = Guid.Parse("a54ef079-5ef6-4867-8a48-38ab8068ed1c"),
            CountryCode = "AT",
            CountryName = "Austria",
            ContestRoles = [V0.Dtos.Countries.ContestRole.CreateExample()],
        };
}
