namespace Eurocentric.Domain.V0.Rankings.Common;

public abstract record RankingsPage<TRanking, TMetadata>(List<TRanking> Rankings, TMetadata Metadata)
    where TRanking : class
    where TMetadata : class;
