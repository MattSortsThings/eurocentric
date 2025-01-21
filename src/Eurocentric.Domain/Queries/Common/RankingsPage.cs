namespace Eurocentric.Domain.Queries.Common;

public abstract record RankingsPage<TItem, TMetadata>(TItem[] Items, TMetadata Metadata)
    where TItem : class
    where TMetadata : class;
