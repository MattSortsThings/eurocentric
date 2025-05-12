using Eurocentric.Domain.Countries;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Enums;

namespace Eurocentric.Features.AdminApi.V1.Countries.Common;

internal static class Mapping
{
    internal static CountryDto ToCountryDto(this Country country) =>
        new(country.Id.Value,
            country.CountryCode.Value,
            country.Name.Value,
            country.ContestMemos.Select(memo => memo.ToContestMemoDto()).ToArray());

    private static ContestMemoDto ToContestMemoDto(this ContestMemo contestMemo) =>
        new(contestMemo.ContestId.Value,
            Enum.Parse<ContestStatus>(contestMemo.Status.ToString()));
}
