namespace Eurocentric.Infrastructure.EFCore;

internal static class EnumHelpers
{
    internal static string GetSqlIntegerListInParentheses<TEnum>()
        where TEnum : Enum => "(" + string.Join(", ", Enum.GetValues(typeof(TEnum)).Cast<int>().Select(v => v.ToString())) + ")";
}
