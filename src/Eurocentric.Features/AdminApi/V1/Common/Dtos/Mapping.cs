using Eurocentric.Features.AdminApi.V1.Common.Enums;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

internal static class Mapping
{
    internal static Country ToCountryDto(this Domain.Countries.Country country) =>
        new(country.Id.Value,
            country.CountryCode.Value,
            country.Name.Value,
            country.ContestMemos.Select(memo => memo.ToContestMemoDto()).ToArray());

    private static ContestMemo ToContestMemoDto(this Domain.ValueObjects.ContestMemo contestMemo) =>
        new(contestMemo.ContestId.Value,
            Enum.Parse<ContestStatus>(contestMemo.Status.ToString()));
}
