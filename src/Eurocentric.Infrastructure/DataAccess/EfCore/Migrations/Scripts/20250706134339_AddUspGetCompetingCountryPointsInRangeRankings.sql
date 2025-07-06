CREATE PROCEDURE dbo.usp_get_competing_country_points_in_range_rankings(
    @min_points_value INT = -999999,
    @max_points_value INT = 999999,
    @include_televote_awards BIT = 0,
    @include_jury_awards BIT = 0,
    @min_contest_year INT = -999999,
    @max_contest_year INT = 999999,
    @contest_stages dbo.tvp_enum_int_value READONLY,
    @voting_country_code NVARCHAR(MAX) = NULL,
    @page_index INT = 0,
    @page_size INT = 10,
    @descending BIT = 0
) AS

BEGIN

    SET NOCOUNT ON;

    WITH data AS
             (SELECT t.competing_country_code,
                     IIF((t.points_value BETWEEN @min_points_value AND @max_points_value), 1.0, 0.0) AS points_in_range,
                     t.voting_country_code,
                     t.broadcast_tag,
                     t.contest_year
              FROM dbo.placeholder_queryable_televote_award t,
                   @contest_stages c
              WHERE (@include_televote_awards = 1)
                AND (t.contest_year BETWEEN @min_points_value AND @max_contest_year)
                AND t.contest_stage = c.value
                AND (@voting_country_code IS NULL OR t.voting_country_code = @voting_country_code)
              UNION ALL
              SELECT j.competing_country_code,
                     IIF((j.points_value BETWEEN @min_points_value AND @max_points_value), 1.0, 0.0) AS points_in_range,
                     j.voting_country_code,
                     j.broadcast_tag,
                     j.contest_year
              FROM dbo.placeholder_queryable_jury_award j,
                   @contest_stages c
              WHERE (@include_jury_awards = 1)
                AND (j.contest_year BETWEEN @min_contest_year AND @max_contest_year)
                AND j.contest_stage = c.value
                AND (@voting_country_code IS NULL OR j.voting_country_code = @voting_country_code)),

         agg AS
             (SELECT data.competing_country_code              AS country_code,
                     AVG(data.points_in_range)                AS points_in_range,
                     COUNT(*)                                 AS points_awards,
                     COUNT(DISTINCT data.broadcast_tag)       AS broadcasts,
                     COUNT(DISTINCT data.contest_year)        AS contests,
                     COUNT(DISTINCT data.voting_country_code) AS voting_countries
              FROM data
              GROUP BY data.competing_country_code)

    SELECT DENSE_RANK() OVER (ORDER BY agg.points_in_range DESC) AS rank,
           agg.country_code,
           agg.points_in_range,
           agg.points_awards,
           agg.broadcasts,
           agg.contests,
           agg.voting_countries
    INTO #ranking
    FROM agg;

    DECLARE
        @total_items INT = (SELECT COUNT(*)
                            FROM #ranking);

    SELECT @page_index  AS page_index,
           @page_size   AS page_size,
           @total_items AS total_items,
           IIF((@total_items % @page_size = 0), (@total_items / @page_size), (@total_items / @page_size) + 1)
                        AS total_pages;

    WITH p AS (SELECT r.rank,
                      r.country_code,
                      r.points_in_range,
                      r.points_awards,
                      r.broadcasts,
                      r.contests,
                      r.voting_countries
               FROM #ranking r
               ORDER BY CASE WHEN @descending = 1 THEN r.rank END DESC,
                        CASE WHEN @descending = 0 THEN r.rank END,
                        r.country_code
               OFFSET (@page_index * @page_size) ROWS FETCH NEXT (@page_size) ROWS ONLY)

    SELECT p.rank,
           p.country_code,
           pqc.country_name,
           p.points_in_range,
           p.points_awards,
           p.broadcasts,
           p.contests,
           p.voting_countries
    FROM p,
         placeholder_queryable_country pqc
    WHERE p.country_code = pqc.country_code;

END

GO
