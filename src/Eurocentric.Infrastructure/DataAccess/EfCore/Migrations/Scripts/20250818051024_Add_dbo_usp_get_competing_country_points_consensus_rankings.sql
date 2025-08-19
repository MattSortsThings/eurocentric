/*
    Creates the stored procedure dbo.usp_get_competing_country_points_consensus_rankings.
*/

CREATE PROCEDURE dbo.usp_get_competing_country_points_consensus_rankings(
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
Procedure Name: dbo.usp_get_competing_country_points_consensus_rankings
Description   : Ranks each competing country by descending POINTS CONSENSUS metric, 
                i.e. the cosine similarity between all the individual jury points awards it has received 
                and all the individual televote points it has received, 
                using each voting country in each broadcast as a vector dimension for comparison. 
                Returns a page of rankings.

Input Parameters:
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
        -- Filter by voting country
        t0 AS (SELECT c.id AS voting_country_id
               FROM dbo.country c
               WHERE @voting_country_code IS NULL
                  OR c.country_code = @voting_country_code),

        -- Filter by contest stage and contest year range
        t1 AS (SELECT b.id AS broadcast_id,
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
        t2 AS (SELECT ja.competing_country_id,
                      t0.voting_country_id,
                      t1.broadcast_id,
                      t1.contest_id,
                      ja.points_value                                                AS jury_points_value,
                      MAX(ja.points_value)
                          OVER (PARTITION BY ja.broadcast_id, ja.voting_country_id ) AS max_jury_points_value
               FROM dbo.broadcast_competitor_jury_award ja,
                    t0,
                    t1
               WHERE ja.voting_country_id = t0.voting_country_id
                 AND ja.broadcast_id = t1.broadcast_id),

        t3 AS (SELECT ta.competing_country_id,
                      t0.voting_country_id,
                      t1.broadcast_id,
                      t1.contest_id,
                      ta.points_value                                                AS televote_points_value,
                      MAX(ta.points_value)
                          OVER (PARTITION BY ta.broadcast_id, ta.voting_country_id ) AS max_televote_points_value
               FROM dbo.broadcast_competitor_televote_award ta,
                    t0,
                    t1
               WHERE ta.voting_country_id = t0.voting_country_id
                 AND ta.broadcast_id = t1.broadcast_id),

        t4 AS (SELECT t2.competing_country_id,
                      t2.voting_country_id,
                      t2.broadcast_id,
                      t2.contest_id,
                      IIF(t2.jury_points_value = 0, 1.0,
                          (100.0 * t2.jury_points_value) / t2.max_jury_points_value)       AS normalized_jury_points_value,
                      IIF(t3.televote_points_value = 0, 1.0, (100.0 * t3.televote_points_value) /
                                                             t3.max_televote_points_value) AS normalized_televote_points_value
               FROM t2,
                    t3
               WHERE t2.competing_country_id = t3.competing_country_id
                 AND t2.voting_country_id = t3.voting_country_id
                 AND t2.broadcast_id = t3.broadcast_id),

        -- Group by and calculate metric
        t5 AS (SELECT t4.competing_country_id,
                      SUM(t4.normalized_jury_points_value * t4.normalized_televote_points_value) AS vector_dot_product,
                      SQRT(SUM(POWER(t4.normalized_jury_points_value, 2)))                       AS jury_vector_length,
                      SQRT(SUM(POWER(t4.normalized_televote_points_value, 2)))                   AS televote_vector_length,
                      COUNT(*)                                                                   AS points_award_pairs,
                      COUNT(DISTINCT t4.broadcast_id)                                            AS broadcasts,
                      COUNT(DISTINCT t4.contest_id)                                              AS contests,
                      COUNT(DISTINCT t4.voting_country_id)                                       AS voting_countries
               FROM t4
               GROUP BY t4.competing_country_id),

        t6 AS (SELECT t5.competing_country_id,
                      t5.vector_dot_product / (t5.jury_vector_length * t5.televote_vector_length) AS points_consensus,
                      t5.vector_dot_product,
                      t5.jury_vector_length,
                      t5.televote_vector_length,
                      t5.points_award_pairs,
                      t5.broadcasts,
                      t5.contests,
                      t5.voting_countries
               FROM t5)

    -- Populate #ranking temp table
    SELECT RANK() OVER (ORDER BY t6.points_consensus DESC) AS rank,
           c.country_code,
           c.country_name,
           t6.points_consensus,
           t6.vector_dot_product,
           t6.jury_vector_length,
           t6.televote_vector_length,
           t6.points_award_pairs,
           t6.broadcasts,
           t6.contests,
           t6.voting_countries
    INTO #ranking
    FROM t6,
         dbo.country c
    WHERE c.id = t6.competing_country_id;

    -- Set @total_items output parameter
    SET @total_items = (SELECT COUNT(*) FROM #ranking);

    -- Return page of rankings
    SELECT r.rank,
           r.country_code,
           r.country_name,
           CAST(ROUND(r.points_consensus, 6) AS DECIMAL(9, 6))        AS points_consensus,
           CAST(ROUND(r.vector_dot_product, 6) AS DECIMAL(18, 6))     AS vector_dot_product,
           CAST(ROUND(r.jury_vector_length, 6) AS DECIMAL(18, 6))     AS jury_vector_length,
           CAST(ROUND(r.televote_vector_length, 6) AS DECIMAL(18, 6)) AS televote_vector_length,
           r.points_award_pairs,
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