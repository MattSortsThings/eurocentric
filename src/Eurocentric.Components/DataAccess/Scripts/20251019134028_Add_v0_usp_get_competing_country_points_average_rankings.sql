SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE v0.usp_get_competing_country_points_average_rankings(
    @min_year INT = NULL,
    @max_year INT = NULL,
    @contest_stage NVARCHAR(10) = NULL,
    @voting_method NVARCHAR(10) = NULL,
    @voting_country_code NCHAR(2) = NULL,
    @page_index INT = 0,
    @page_size INT = 10,
    @descending BIT = FALSE
) AS

BEGIN

    SET NOCOUNT ON;

    DECLARE @resolved_min_year INT = COALESCE(@min_year, -999999);
    DECLARE @resolved_max_year INT = COALESCE(@max_year, 999999);
    DECLARE @resolved_voting_method NVARCHAR(1) = COALESCE(@voting_method, N'Any');

    WITH

--------------- Selecting query data ----------------

filtered_broadcast AS (SELECT k.contest_id,
                              b.broadcast_id
                       FROM v0.contest k
                                JOIN v0.broadcast b ON b.parent_contest_id = k.contest_id
                                INNER JOIN (SELECT contest_stage
                                            FROM dbo.tvf_map_contest_stage_filter_enum(@contest_stage)) cs
                                           ON cs.contest_stage = b.contest_stage
                       WHERE k.queryable = 1
                         AND k.contest_year BETWEEN @resolved_min_year AND @resolved_max_year),

filtered_voting_country AS (SELECT c.country_id AS voting_country_id
                            FROM v0.country c
                            WHERE @voting_country_code IS NULL
                               OR @voting_country_code = c.country_code),

query_datum AS (SELECT bcja.competing_country_id,
                       bcja.broadcast_id,
                       bcja.voting_country_id,
                       fb.contest_id,
                       bcja.points_value
                FROM v0.broadcast_competitor_jury_award bcja
                         JOIN filtered_broadcast fb ON fb.broadcast_id = bcja.broadcast_id
                         JOIN filtered_voting_country fvc ON fvc.voting_country_id = bcja.voting_country_id
                WHERE @resolved_voting_method <> N'Televote'

                UNION ALL

                SELECT bcta.competing_country_id,
                       bcta.broadcast_id,
                       bcta.voting_country_id,
                       fb.contest_id,
                       bcta.points_value
                FROM v0.broadcast_competitor_televote_award bcta
                         JOIN filtered_broadcast fb ON fb.broadcast_id = bcta.broadcast_id
                         JOIN filtered_voting_country fvc ON fvc.voting_country_id = bcta.voting_country_id
                WHERE @resolved_voting_method <> N'Jury'),

-------- Grouping data by competing country ---------

grouped_datum AS (SELECT qd.competing_country_id,
                         SUM(qd.points_value)                 AS total_points,
                         COUNT(*)                             AS points_awards,
                         COUNT(DISTINCT qd.broadcast_id)      AS broadcasts,
                         COUNT(DISTINCT qd.contest_id)        AS contests,
                         COUNT(DISTINCT qd.voting_country_id) AS voting_countries
                  FROM query_datum qd
                  GROUP BY qd.competing_country_id),

-------- Calculating POINTS_AVERAGE metrics ---------

grouped_metric AS (SELECT gd.competing_country_id,
                          CAST(((1.0 * gd.total_points) / gd.points_awards) AS DECIMAL(9, 6)) AS points_average,
                          gd.total_points,
                          gd.points_awards,
                          gd.broadcasts,
                          gd.contests,
                          gd.voting_countries
                   FROM grouped_datum gd)

------- Ranking by descending POINTS_AVERAGE- -------

    SELECT RANK() OVER (ORDER BY gm.points_average DESC) AS rank,
           c.country_code,
           c.country_name,
           gm.points_average,
           gm.total_points,
           gm.points_awards,
           gm.broadcasts,
           gm.contests,
           gm.voting_countries
    INTO #ranking
    FROM grouped_metric gm
             JOIN v0.country c ON c.country_id = gm.competing_country_id;

-------- Returning sorted paginated rankings --------

    SELECT r.rank,
           r.country_code,
           r.country_name,
           r.points_average,
           r.total_points,
           r.points_awards,
           r.broadcasts,
           r.contests,
           r.voting_countries
    FROM #ranking r
    ORDER BY CASE WHEN @descending = 1 THEN r.rank END DESC,
             CASE WHEN @descending = 0 THEN r.rank END,
             r.country_code
    OFFSET (@page_index * @page_size) ROWS FETCH NEXT (@page_size) ROWS ONLY;

------------- Returning rankings metadata ------------

    DECLARE @total_items INT = (SELECT COUNT(*) FROM #ranking);
    DECLARE @total_pages INT = IIF(@total_items = 0, 0, CEILING((1.0 * @total_items) / @page_size));

    SELECT @min_year            AS min_year,
           @max_year            AS max_year,
           @contest_stage       AS contest_stage,
           @voting_method       AS voting_method,
           @voting_country_code AS voting_country_code,
           @page_index          AS page_index,
           @page_size           AS page_size,
           @descending          AS descending,
           @total_items         AS total_items,
           @total_pages         AS total_pages;

END

GO
