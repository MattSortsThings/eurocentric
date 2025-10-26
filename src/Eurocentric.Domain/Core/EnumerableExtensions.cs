using CSharpFunctionalExtensions;

namespace Eurocentric.Domain.Core;

public static class CollectionExtensions
{
    public static Result<List<TItem>, TError> Collect<TItem, TError>(this ICollection<Result<TItem, TError>> results)
    {
        List<TItem> items = new(results.Count);

        foreach (Result<TItem, TError> result in results)
        {
            if (result.IsSuccess)
            {
                items.Add(result.Value);
            }
            else
            {
                return result.Error;
            }
        }

        return items;
    }
}
