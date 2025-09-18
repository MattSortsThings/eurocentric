/*
    Creates the stored procedure v0.usp_get_scoreboard_rows.
*/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE v0.usp_get_scoreboard_rows(
    @contest_year INT,
    @contest_stage INT
)
AS
/*
-- =============================================
-- Procedure:   v0.usp_get_scoreboard_rows
-- Author:      Matt Tantony
-- Create date: 2025-09-18
--
-- Description:
--  Returns the scoreboard rows from the queryable broadcast matching the input parameters, if it exists.
--
--  Returned scoreboard rows are ordered by finishing position. The procedure returns an empty set of records if no
--  queryable broadcast matches the input parameters.
--
-- Input parameters:
--  @contest_year INT - Sets the contest year.
--  @contest_stage INT - Sets the contest stage enum integer value.
--
-- Returns a page of records with the following columns:
--  - finishing_position INT
--  - running_order_spot INT
--  - country_code NCHAR(2)
--  - country_name NVARCHAR(200)
--  - act_name NVARCHAR(200)
--  - song_title NVARCHAR(200)
--  - overall_points INT
--  - jury_points INT (NULL if broadcast has no juries)
--  - televote_points INT
-- =============================================
*/

BEGIN

    SET NOCOUNT ON;

    --------------- Filtering voting data ---------------

    WITH queried_competitor AS (SELECT c.broadcast_id,
                                       c.competing_country_id,
                                       c.running_order_spot,
                                       c.finishing_position
                                FROM v0.contest e
                                         JOIN v0.broadcast b ON b.parent_contest_id = e.contest_id
                                         JOIN v0.competitor c ON c.broadcast_id = b.broadcast_id
                                WHERE e.queryable = 1
                                  AND e.contest_year = @contest_year
                                  AND b.contest_stage = @contest_stage),

         query_datum AS (SELECT qc.broadcast_id,
                                qc.competing_country_id,
                                qc.finishing_position,
                                qc.running_order_spot,
                                ta.points_value AS televote_points_value,
                                ja.points_value AS jury_points_value
                         FROM queried_competitor qc
                                  JOIN v0.televote_award ta ON ta.broadcast_id = qc.broadcast_id AND
                                                               ta.competing_country_id = qc.competing_country_id
                                  LEFT JOIN v0.jury_award ja ON ja.broadcast_id = ta.broadcast_id AND
                                                                ja.competing_country_id = ta.competing_country_id AND
                                                                ja.voting_country_id = ta.voting_country_id),

         ------------- Grouping data by competitor -----------

         grouped_datum AS (SELECT qd.broadcast_id,
                                  qd.competing_country_id,
                                  qd.finishing_position,
                                  qd.running_order_spot,
                                  SUM(qd.televote_points_value) AS televote_points,
                                  SUM(qd.jury_points_value)     AS jury_points
                           FROM query_datum qd
                           GROUP BY qd.broadcast_id,
                                    qd.competing_country_id,
                                    qd.finishing_position,
                                    qd.running_order_spot)

    --------- Creating scoreboard rows ----------

    SELECT gd.finishing_position,
           gd.running_order_spot,
           c.country_code,
           c.country_name,
           p.act_name,
           p.song_title,
           COALESCE(gd.jury_points, 0) + gd.televote_points AS overall_points,
           gd.jury_points,
           gd.televote_points
    FROM grouped_datum gd
             JOIN v0.child_broadcast cb ON cb.child_broadcast_id = gd.broadcast_id
             JOIN v0.participant p
                  ON p.contest_id = cb.contest_id AND p.participating_country_id = gd.competing_country_id
             JOIN v0.country c ON c.country_id = gd.competing_country_id
    ORDER BY gd.finishing_position;

END
GO
