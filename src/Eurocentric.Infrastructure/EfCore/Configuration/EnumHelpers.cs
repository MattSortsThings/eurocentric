namespace Eurocentric.Infrastructure.EfCore.Configuration;

internal static class EnumHelpers
{
    internal static string GetSqlNameListInParentheses<TEnum>()
        where TEnum : Enum => "(" + string.Join(", ", Enum.GetNames(typeof(TEnum)).Select(n => $"N'{n}'")) + ")";

    internal static string GetSqlIntegerListInParentheses<TEnum>()
        where TEnum : Enum => "(" + string.Join(", ", Enum.GetValues(typeof(TEnum)).Cast<int>().Select(v => v.ToString())) + ")";
}
