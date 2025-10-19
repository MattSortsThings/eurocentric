SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE v0.usp_get_broadcast_result_listings(
    @contest_year INT,
    @contest_stage NVARCHAR(10)
)
AS
BEGIN
    SET NOCOUNT ON;

    WITH filtered_broadcast AS (SELECT *
                                FROM v0.broadcast b
                                         JOIN v0.contest k ON k.contest_id = b.parent_contest_id
                                WHERE k.queryable = 1
                                  AND k.contest_year = @contest_year
                                  AND b.contest_stage = @contest_stage),

         filtered_jury_points AS (SELECT bcja.broadcast_id,
                                         bcja.competing_country_id,
                                         SUM(bcja.points_value) AS jury_points
                                  FROM v0.broadcast_competitor_jury_award bcja
                                           JOIN filtered_broadcast fb ON fb.broadcast_id = bcja.broadcast_id
                                  GROUP BY bcja.broadcast_id, bcja.competing_country_id),

         filtered_televote_points AS (SELECT bcta.broadcast_id,
                                             bcta.competing_country_id,
                                             SUM(bcta.points_value) AS televote_points
                                      FROM v0.broadcast_competitor_televote_award bcta
                                               JOIN filtered_broadcast fb ON fb.broadcast_id = bcta.broadcast_id
                                      GROUP BY bcta.broadcast_id, bcta.competing_country_id),

         raw_listing AS (SELECT fb.contest_id,
                                bc.competing_country_id,
                                bc.running_order_spot,
                                fjp.jury_points,
                                ftp.televote_points,
                                COALESCE(fjp.jury_points, 0) + ftp.televote_points AS overall_points,
                                RANK() OVER (ORDER BY fjp.jury_points DESC)        AS jury_rank,
                                RANK() OVER (ORDER BY ftp.televote_points DESC)    AS televote_rank,
                                bc.finishing_position
                         FROM filtered_broadcast fb
                                  JOIN v0.broadcast_competitor bc ON bc.broadcast_id = fb.broadcast_id
                                  LEFT OUTER JOIN filtered_jury_points fjp ON fjp.broadcast_id = bc.broadcast_id AND
                                                                              fjp.competing_country_id =
                                                                              bc.competing_country_id
                                  LEFT OUTER JOIN filtered_televote_points ftp ON ftp.broadcast_id = bc.broadcast_id AND
                                                                                  ftp.competing_country_id =
                                                                                  bc.competing_country_id)


    SELECT rl.running_order_spot,
           c.country_code,
           c.country_name,
           cp.act_name,
           cp.song_title,
           rl.jury_points,
           rl.televote_points,
           rl.overall_points,
           IIF(rl.jury_points IS NULL, NULL, rl.jury_rank) AS jury_rank,
           rl.televote_rank,
           rl.finishing_position
    FROM raw_listing rl
             JOIN v0.country c ON c.country_id = rl.competing_country_id
             JOIN v0.contest_participant cp
                  ON cp.contest_id = rl.contest_id AND cp.participating_country_id = rl.competing_country_id
    ORDER BY rl.finishing_position;

END
GO
