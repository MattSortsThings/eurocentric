using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V0.Common.Dtos;

public sealed record Country : IExampleProvider<Country>
{
    public required Guid Id { get; init; }

    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public required Guid[] ParticipatingContestIds { get; init; }

    public static Country CreateExample() => new()
    {
        Id = Guid.Parse("44aab3c9-f7d1-44d6-bc30-641511ea65a6"),
        CountryCode = "AT",
        CountryName = "Austria",
        ParticipatingContestIds = []
    };
}
