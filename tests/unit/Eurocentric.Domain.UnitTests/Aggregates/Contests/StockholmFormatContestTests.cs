using ErrorOr;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Aggregates.Contests;

public sealed partial class StockholmFormatContestTests : UnitTest
{
    private const string DateFormat = "yyyy-MM-dd";

    private static readonly ContestYear ContestYear2025 = ContestYear.FromValue(2025).Value;

    private static readonly BroadcastDate BroadcastDate2025May1 =
        BroadcastDate.FromValue(DateOnly.ParseExact("2025-05-01", DateFormat)).Value;

    private static readonly CityName ArbitraryCityName = CityName.FromValue("CityName").Value;
    private static readonly ActName ArbitraryActName = ActName.FromValue("ActName").Value;
    private static readonly SongTitle ArbitrarySongTitle = SongTitle.FromValue("SongTitle").Value;

    private static readonly ContestId FixedContestId = ContestId.FromValue(Guid.Parse("10c007f3-9eea-4703-bb56-e1ac8a3bffa3"));

    private static readonly BroadcastId FixedBroadcastId =
        BroadcastId.FromValue(Guid.Parse("eb3036eb-a2b3-450f-b57b-2bfd4516b523"));

    private static readonly CountryId AtId = CountryId.FromValue(Guid.Parse("01abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId BeId = CountryId.FromValue(Guid.Parse("02abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId CzId = CountryId.FromValue(Guid.Parse("03abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId DkId = CountryId.FromValue(Guid.Parse("04abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId EeId = CountryId.FromValue(Guid.Parse("05abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId FiId = CountryId.FromValue(Guid.Parse("06abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId GbId = CountryId.FromValue(Guid.Parse("07abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId XxId = CountryId.FromValue(Guid.Parse("24abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));

    private static StockholmFormatContest CreateContest(CountryId[] group2CountryIds = null!,
        CountryId[] group1CountryIds = null!,
        ContestYear? contestYear = null,
        ContestId? contestId = null)
    {
        Func<ContestId> idProvider = contestId is not null
            ? () => contestId
            : () => FixedContestId;

        ErrorOr<ContestYear> errorsOrContestYear = contestYear ?? ContestYear2025;

        ContestBuilder builder = StockholmFormatContest.Create()
            .WithContestYear(errorsOrContestYear)
            .WithCityName(ArbitraryCityName);

        foreach (CountryId countryId in group1CountryIds)
        {
            builder.AddGroup1Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
        }

        foreach (CountryId countryId in group2CountryIds)
        {
            builder.AddGroup2Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
        }

        ErrorOr<Contest> result = builder.Build(idProvider);

        return result.Value as StockholmFormatContest ?? throw new InvalidCastException("Contest not created");
    }
}
