namespace Eurocentric.Domain.Placeholders.Aggregates;

public sealed class Country
{
    public required Guid Id { get; init; }

    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public required List<Guid> CountryIds { get; init; }
}
