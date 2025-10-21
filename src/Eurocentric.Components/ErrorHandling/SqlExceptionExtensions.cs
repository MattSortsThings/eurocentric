using Microsoft.Data.SqlClient;

namespace Eurocentric.Components.ErrorHandling;

internal static class SqlExceptionExtensions
{
    internal static bool CausedByDbTimeout(this SqlException exception) =>
        exception.Message.Contains("Timeout") || exception.Message.Contains("timeout");
}
