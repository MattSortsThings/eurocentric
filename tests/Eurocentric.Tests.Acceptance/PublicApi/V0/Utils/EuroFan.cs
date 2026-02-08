using Eurocentric.Tests.Acceptance.Utils.Parsers;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Tests.Acceptance.PublicApi.V0.Utils;

public abstract partial class EuroFan
{
    protected EuroFan(IEuroFanKernel kernel)
    {
        Kernel = kernel;
    }

    private protected IEuroFanKernel Kernel { get; }

    private protected RestRequest? Request { get; set; }

    private protected RestResponse<ProblemDetails>? FailureResponse { get; set; }

    public async Task Given_the_system_contains_all_53_countries() =>
        await Kernel.BackDoor.ExecuteScopedAsync(BackDoorOperations.AddAll53CountriesAsync);

    public async Task Given_the_system_contains_these_contests(string table)
    {
        ContestDatum[] contestData = MarkdownParser.ParseTable(table, MapRowToContestDatum);

        await Kernel.BackDoor.ExecuteScopedAsync(BackDoorOperations.AddContestsAsync(contestData));
    }

    public abstract Task When_I_send_my_request();

    private static ContestDatum MapRowToContestDatum(Dictionary<string, string> row)
    {
        int contestYear = int.Parse(row["contest_year"]);
        string[] completedContestStages = row["completed_contest_stages"].Split(", ");

        return new ContestDatum(contestYear, completedContestStages);
    }
}
