using System.Data;
using Dapper;
using Eurocentric.Components.DataAccess.Dapper;
using Eurocentric.Tests.Acceptance.Utils.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Tests.Acceptance.PublicApi.V0.Utils;

public abstract partial class EuroFan
{
    private static (string DbSprocName, DynamicParameters DbSprocParameters) Map(ContestDatum contestDatum) =>
        (MapToDbSprocName(contestDatum.contestYear), MapToDbSprocParameters(contestDatum.completedContestStages));

    private static string MapToDbSprocName(int contestYear)
    {
        return contestYear switch
        {
            2021 => DbSprocs.Placeholder.Add2021Contest,
            2022 => DbSprocs.Placeholder.Add2022Contest,
            2023 => DbSprocs.Placeholder.Add2023Contest,
            2024 => DbSprocs.Placeholder.Add2024Contest,
            _ => throw new NotSupportedException($"Value {contestYear} is not supported."),
        };
    }

    private static DynamicParameters MapToDbSprocParameters(string[] completedContestStages)
    {
        DataTable dataTable = new();
        dataTable.Columns.Add("contest_stage", typeof(string));
        foreach (string contestStage in completedContestStages)
        {
            dataTable.Rows.Add(contestStage);
        }

        DynamicParameters parameters = new();
        parameters.Add("@completed_contest_stages", dataTable.AsTableValuedParameter());

        return parameters;
    }

    private static class BackDoorOperations
    {
        public static async Task AddAll53CountriesAsync(IServiceProvider serviceProvider)
        {
            DbSprocRunner dbSprocRunner = serviceProvider.GetRequiredService<DbSprocRunner>();
            await dbSprocRunner.ExecuteAsync(DbSprocs.Placeholder.AddAll53Countries);
        }

        public static Func<IServiceProvider, Task> AddContestsAsync(ContestDatum[] contestData)
        {
            ContestDatum[] data = contestData;

            return async serviceProvider =>
            {
                DbSprocRunner dbSprocRunner = serviceProvider.GetRequiredService<DbSprocRunner>();

                await Task.WhenAll(
                    data.Select(Map)
                        .Select(tuple => dbSprocRunner.ExecuteAsync(tuple.DbSprocName, tuple.DbSprocParameters))
                );
            };
        }
    }

    private readonly record struct ContestDatum(int contestYear, string[] completedContestStages);
}
