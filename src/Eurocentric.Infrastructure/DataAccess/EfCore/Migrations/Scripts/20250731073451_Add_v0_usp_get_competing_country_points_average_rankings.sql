/*
    Creates the stored procedure v0.usp_get_competing_country_points_average_rankings.
*/

CREATE PROCEDURE v0.usp_get_competing_country_points_average_rankings(
    @exclude_jury BIT = 0,
    @exclude_televote BIT = 0,
    @min_year INT = NULL,
    @max_year INT = NULL,
    @contest_stages dbo.tvp_enum_int_value READONLY,
    @voting_country_code NVARCHAR(200) = NULL,
    @page_index INT = 0,
    @page_size INT = 10,
    @descending BIT = 0,
    @total_items INT OUTPUT
)
AS
/*
    Procedure Name: v0.usp_get_competing_country_points_average_rankings
    Description   : Ranks each competing country by descending average points value of all the individual points awards
                    it has received. Returns a page of rankings.

    Input Parameters:
        @exclude_jury BIT - Excludes jury awards from the queried data.
        @exclude_televote BIT - Excludes televote awards from the queried data.
        @min_year INT - Filters the queried data by inclusive minimum contest year.
        @max_year INT - Filters the queried data by inclusive maximum contest year.
        @contest_stages TVP - Filters the queried data by contest stage.
        @voting_country_code NVARCHAR(MAX) - Filters the queried data by voting country code.

        @page_index INT - Zero-based pagination page index.
        @page_size INT - Maximum number of records per page for pagination.
        @descending BIT - Orders rankings by descending rank before pagination.

    Output Parameters:
        @total_items INT - Is set to the total number of rankings before pagination.

    Returns:
        Ordered page of rankings.

    Notes:
        - Sets the @total_items output parameter.
*/

BEGIN

    SET NOCOUNT ON;

    WITH
        -- Filtered voting country IDs
        t0 AS (SELECT c.id AS voting_country_id
               FROM v0.country c
               WHERE @voting_country_code IS NULL
                  OR c.country_code = @voting_country_code),

        -- Filtered broadcast IDs and contest IDs
        t1 AS (SELECT b.id AS broadcast_id,
                      e.id AS contest_id
               FROM v0.contest e,
                    v0.broadcast b,
                    @contest_stages cs
               WHERE e.completed = 1
                 AND (@min_year IS NULL OR e.contest_year >= @min_year)
                 AND (@max_year IS NULL OR e.contest_year <= @max_year)
                 AND e.id = b.parent_contest_id
                 AND b.contest_stage = cs.value),

        -- Queryable data
        t2 AS (SELECT ja.competing_country_id,
                      t0.voting_country_id,
                      t1.broadcast_id,
                      t1.contest_id,
                      CAST(ja.points_value AS FLOAT) AS real_points_value
               FROM v0.broadcast_competitor_jury_award ja,
                    t0,
                    t1
               WHERE @exclude_jury = 0
                 AND ja.voting_country_id = t0.voting_country_id
                 AND ja.broadcast_id = t1.broadcast_id
               UNION ALL
               SELECT ta.competing_country_id,
                      t0.voting_country_id,
                      t1.broadcast_id,
                      t1.contest_id,
                      CAST(ta.points_value AS FLOAT) AS real_points_value
               FROM v0.broadcast_competitor_televote_award ta,
                    t0,
                    t1
               WHERE @exclude_televote = 0
                 AND ta.voting_country_id = t0.voting_country_id
                 AND ta.broadcast_id = t1.broadcast_id),

        -- Aggregate
        t3 AS (SELECT t2.competing_country_id,
                      AVG(t2.real_points_value)            AS points_average,
                      COUNT(*)                             AS points_awards,
                      COUNT(DISTINCT t2.broadcast_id)      AS broadcasts,
                      COUNT(DISTINCT t2.contest_id)        AS contests,
                      COUNT(DISTINCT t2.voting_country_id) AS voting_countries
               FROM t2
               GROUP BY t2.competing_country_id)

    -- Ranking
    SELECT RANK() OVER (ORDER BY t3.points_average DESC) AS rank,
           c.country_code,
           c.country_name,
           t3.points_average,
           t3.points_awards,
           t3.broadcasts,
           t3.contests,
           t3.voting_countries
    INTO #ranking
    FROM t3,
         v0.country c
    WHERE t3.competing_country_id = c.id;

    -- Total items
    SET @total_items = (SELECT COUNT(*) FROM #ranking);

    -- Page
    SELECT r.rank,
           r.country_code,
           r.country_name,
           r.points_average,
           r.points_awards,
           r.broadcasts,
           r.contests,
           r.voting_countries
    FROM #ranking r
    ORDER BY CASE WHEN @descending = 1 THEN r.rank END DESC,
             CASE WHEN @descending = 0 THEN r.rank END,
             r.country_code
    OFFSET (@page_index * @page_size) ROWS FETCH NEXT (@page_size) ROWS ONLY;

END
GO
