namespace Eurocentric.Infrastructure.DataAccess.EfCore.Configuration;

internal static class SqlHelpers
{
    internal static string CreateEnumIntValueCheckConstraint<T>(string columnName)
        where T : Enum
    {
        IEnumerable<int> intValues = Enum.GetValuesAsUnderlyingType(typeof(T))
            .Cast<int>()
            .Distinct();

        return $"[{columnName}] IN ({string.Join(",", intValues)})";
    }
}
