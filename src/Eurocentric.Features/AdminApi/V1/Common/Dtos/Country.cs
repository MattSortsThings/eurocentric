using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Country : IExampleProvider<Country>
{
    public required Guid Id { get; init; }

    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public required Guid[] ParticipatingContestIds { get; init; }

    public static Country CreateExample() => new()
    {
        Id = ExampleValues.CountryId,
        CountryCode = "AT",
        CountryName = "Austria",
        ParticipatingContestIds = [ExampleValues.ContestId]
    };
}
