/*
    Creates the stored procedure dbo.usp_get_competitor_points_share_rankings.
*/

CREATE PROCEDURE dbo.usp_get_competitor_points_share_rankings(
    @exclude_jury BIT = 0,
    @exclude_televote BIT = 0,
    @min_year INT = NULL,
    @max_year INT = NULL,
    @contest_stages dbo.tvp_enum_int_value READONLY,
    @page_index INT = 0,
    @page_size INT = 10,
    @descending BIT = 0,
    @total_items INT OUTPUT
)
AS

/*
Procedure Name: dbo.usp_get_competitor_points_share_rankings
Description   : Ranks each competitor in each contest broadcast by descending POINTS SHARE metric,
                i.e. the total points it has received as a fraction of the available points.
                Returns a page of rankings.

Input Parameters:
    @exclude_jury BIT - Excludes jury awards from the queried data.
    @exclude_televote BIT - Excludes televote awards from the queried data.
    @min_year INT - Filters the queried data by inclusive minimum contest year.
    @max_year INT - Filters the queried data by inclusive maximum contest year.
    @contest_stages TVP - Filters the queried data by contest stage.

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
        -- Filter by contest stage and contest year range
        t0 AS (SELECT b.id AS broadcast_id,
                      e.id AS contest_id
               FROM dbo.contest e,
                    dbo.broadcast b,
                    @contest_stages cs
               WHERE e.completed = 1
                 AND (@min_year IS NULL OR e.contest_year >= @min_year)
                 AND (@max_year IS NULL OR e.contest_year <= @max_year)
                 AND e.id = b.parent_contest_id
                 AND b.contest_stage = cs.value),

        -- Get queryable data
        t1 AS (SELECT ja.competing_country_id,
                      t0.broadcast_id,
                      t0.contest_id,
                      ja.points_value,
                      MAX(ja.points_value)
                          OVER (PARTITION BY ja.broadcast_id, ja.voting_country_id ) AS max_possible_points
               FROM dbo.broadcast_competitor_jury_award ja,
                    t0
               WHERE @exclude_jury = 0
                 AND ja.broadcast_id = t0.broadcast_id
               UNION ALL
               SELECT ta.competing_country_id,
                      t0.broadcast_id,
                      t0.contest_id,
                      ta.points_value,
                      MAX(ta.points_value)
                          OVER (PARTITION BY ta.broadcast_id, ta.voting_country_id ) AS max_possible_points
               FROM dbo.broadcast_competitor_televote_award ta,
                    t0
               WHERE @exclude_televote = 0
                 AND ta.broadcast_id = t0.broadcast_id),

        -- Group by and calculate metric
        t2 AS (SELECT t1.competing_country_id,
                      t1.broadcast_id,
                      t1.contest_id,
                      SUM(t1.points_value)        AS total_points,
                      SUM(t1.max_possible_points) AS available_points,
                      COUNT(*)                    AS points_awards
               FROM t1
               GROUP BY t1.competing_country_id, t1.broadcast_id, t1.contest_id),

        t3 AS (SELECT t2.competing_country_id,
                      t2.broadcast_id,
                      t2.contest_id,
                      CAST(t2.total_points AS FLOAT) / t2.available_points AS points_share,
                      t2.total_points,
                      t2.available_points,
                      t2.points_awards
               FROM t2)

    -- Populate #ranking temp table
    SELECT RANK() OVER (ORDER BY t3.points_share DESC) AS rank,
           e.contest_year,
           b.contest_stage,
           bc.running_order_position,
           cc.country_code,
           cc.country_name,
           bc.finishing_position,
           cp.act_name,
           cp.song_title,
           t3.points_share,
           t3.total_points,
           t3.available_points,
           t3.points_awards
    INTO #ranking
    FROM t3,
         dbo.broadcast b,
         dbo.contest e,
         dbo.country cc,
         dbo.broadcast_competitor bc,
         dbo.contest_participant cp
    WHERE b.id = t3.broadcast_id
      AND e.id = t3.contest_id
      AND bc.broadcast_id = t3.broadcast_id
      AND bc.competing_country_id = t3.competing_country_id
      AND cc.id = t3.competing_country_id
      AND cp.contest_id = t3.contest_id
      AND cp.participating_country_id = t3.competing_country_id;

    -- Set @total_items output parameter
    SET @total_items = (SELECT COUNT(*) FROM #ranking);

    -- Return page of rankings
    SELECT r.rank,
           r.contest_year,
           r.contest_stage,
           r.running_order_position,
           r.country_code,
           r.country_name,
           r.finishing_position,
           r.act_name,
           r.song_title,
           CAST(ROUND(r.points_share, 6) AS DECIMAL(9, 6)) AS points_share,
           r.total_points,
           r.available_points,
           r.points_awards
    FROM #ranking r
    ORDER BY CASE WHEN @descending = 1 THEN r.rank END DESC,
             CASE WHEN @descending = 0 THEN r.rank END,
             r.contest_year,
             r.contest_stage,
             r.running_order_position
    OFFSET (@page_index * @page_size) ROWS FETCH NEXT (@page_size) ROWS ONLY;

END
GO