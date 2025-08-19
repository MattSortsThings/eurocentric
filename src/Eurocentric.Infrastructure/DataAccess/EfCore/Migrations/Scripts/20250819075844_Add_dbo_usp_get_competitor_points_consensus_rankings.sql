/*
    Creates the stored procedure dbo.usp_get_competitor_points_consensus_rankings.
*/

CREATE PROCEDURE dbo.usp_get_competitor_points_consensus_rankings(
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
Procedure Name: dbo.usp_get_competitor_points_consensus_rankings
Description   : Ranks each competitor in each contest broadcast by descending POINTS CONSENSUS metric, i.e. the cosine 
                similarity between all the individual jury points awards it received and all the individual televote 
                points it received, using each voting country in its broadcast as a vector dimension for comparison. 
                Returns a page of rankings.

Input Parameters:
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
                      ja.voting_country_id,
                      ja.broadcast_id,
                      t0.contest_id,
                      ja.points_value                                                AS jury_points_value,
                      MAX(ja.points_value)
                          OVER (PARTITION BY ja.broadcast_id, ja.voting_country_id ) AS max_jury_points_value
               FROM dbo.broadcast_competitor_jury_award ja,
                    t0
               WHERE ja.broadcast_id = t0.broadcast_id),

        t2 AS (SELECT ta.competing_country_id,
                      ta.voting_country_id,
                      ta.broadcast_id,
                      t0.contest_id,
                      ta.points_value                                                AS televote_points_value,
                      MAX(ta.points_value)
                          OVER (PARTITION BY ta.broadcast_id, ta.voting_country_id ) AS max_televote_points_value
               FROM dbo.broadcast_competitor_televote_award ta,
                    t0
               WHERE ta.broadcast_id = t0.broadcast_id),

        t3 AS (SELECT t1.competing_country_id,
                      t1.broadcast_id,
                      t1.contest_id,
                      IIF(t1.jury_points_value = 0, 1.0,
                          (100.0 * t1.jury_points_value) / t1.max_jury_points_value)         AS normalized_jury_points_value,
                      IIF(t2.televote_points_value = 0, 1.0, (100.0 * t2.televote_points_value) /
                                                             t2.max_televote_points_value)   AS normalized_televote_points_value
               FROM t1,
                    t2
               WHERE t1.competing_country_id = t2.competing_country_id
                 AND t1.voting_country_id = t2.voting_country_id
                 AND t1.broadcast_id = t2.broadcast_id),

        -- Group by and calculate metric
        t4 AS (SELECT t3.competing_country_id,
                      t3.broadcast_id,
                      t3.contest_id,
                      SUM(t3.normalized_jury_points_value * t3.normalized_televote_points_value) AS vector_dot_product,
                      SQRT(SUM(POWER(t3.normalized_jury_points_value, 2)))                       AS jury_vector_length,
                      SQRT(SUM(POWER(t3.normalized_televote_points_value, 2)))                   AS televote_vector_length,
                      COUNT(*)                                                                   AS points_award_pairs
               FROM t3
               GROUP BY t3.competing_country_id, t3.broadcast_id, t3.contest_id),

        t5 AS (SELECT t4.competing_country_id,
                      t4.broadcast_id,
                      t4.contest_id,
                      t4.vector_dot_product / (t4.jury_vector_length * t4.televote_vector_length) AS points_consensus,
                      t4.vector_dot_product,
                      t4.jury_vector_length,
                      t4.televote_vector_length,
                      t4.points_award_pairs
               FROM t4)

    -- Populate #ranking temp table
    SELECT RANK() OVER (ORDER BY t5.points_consensus DESC) AS rank,
           e.contest_year,
           b.contest_stage,
           bc.running_order_position,
           cc.country_code,
           cc.country_name,
           bc.finishing_position,
           cp.act_name,
           cp.song_title,
           t5.points_consensus,
           t5.vector_dot_product,
           t5.jury_vector_length,
           t5.televote_vector_length,
           t5.points_award_pairs
    INTO #ranking
    FROM t5,
         dbo.broadcast b,
         dbo.contest e,
         dbo.country cc,
         dbo.broadcast_competitor bc,
         dbo.contest_participant cp
    WHERE b.id = t5.broadcast_id
      AND e.id = t5.contest_id
      AND bc.broadcast_id = t5.broadcast_id
      AND bc.competing_country_id = t5.competing_country_id
      AND cc.id = t5.competing_country_id
      AND cp.contest_id = t5.contest_id
      AND cp.participating_country_id = t5.competing_country_id;

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
           CAST(ROUND(r.points_consensus, 6) AS DECIMAL(9, 6))        AS points_consensus,
           CAST(ROUND(r.vector_dot_product, 6) AS DECIMAL(18, 6))     AS vector_dot_product,
           CAST(ROUND(r.jury_vector_length, 6) AS DECIMAL(18, 6))     AS jury_vector_length,
           CAST(ROUND(r.televote_vector_length, 6) AS DECIMAL(18, 6)) AS televote_vector_length,
           r.points_award_pairs
    FROM #ranking r
    ORDER BY CASE WHEN @descending = 1 THEN r.rank END DESC,
             CASE WHEN @descending = 0 THEN r.rank END,
             r.contest_year,
             r.contest_stage,
             r.running_order_position
    OFFSET (@page_index * @page_size) ROWS FETCH NEXT (@page_size) ROWS ONLY;

END
GO