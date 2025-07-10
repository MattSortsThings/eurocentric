using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Eurocentric.Infrastructure.DataAccess.Dapper;

internal sealed class DbStoredProcedureRunner(string connectionString, int commandTimeoutInSeconds) :
    IDbStoredProcedureRunner
{
    /// <inheritdoc />
    public async Task<(T1 First, T2[] Second)> ExecuteAsync<T1, T2>(string storedProcedureName,
        DynamicParameters? parameters = null,
        CancellationToken cancellationToken = default) where T1 : class where T2 : class
    {
        await using SqlConnection dbConnection = new(connectionString);

        await dbConnection.OpenAsync(cancellationToken);

        await using SqlMapper.GridReader multi = await dbConnection.QueryMultipleAsync(
            storedProcedureName,
            commandType: CommandType.StoredProcedure,
            param: parameters,
            commandTimeout: commandTimeoutInSeconds);

        T1 firstItem = await multi.ReadSingleAsync<T1>();
        T2[] secondItems = (await multi.ReadAsync<T2>()).ToArray();

        dbConnection.Close();

        return (firstItem, secondItems);
    }
}
