SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE VIEW v0.vw_queryable_broadcast
AS

/*
-- =============================================
-- View:        v0.vw_queryable_broadcast
-- Author:      Matt Tantony
-- Create date: 2025-09-25
--
-- Description:
--  Comprises a summary record for every queryable broadcast in the v0 schema.
--
-- Records have the following columns:
--  - broadcast_date DATE
--  - contest_year INT
--  - contest_stage NVARCHAR(10) (ContestStage enum string value)
--  - competitors INT
--  - juries INT
--  - televotes INT
-- =============================================
*/

WITH competitor_count AS (SELECT c.broadcast_id,
                                 COUNT(*) AS competitors
                          FROM v0.competitor c
                          GROUP BY c.broadcast_id),

     jury_count AS (SELECT j.broadcast_id,
                           COUNT(*) AS juries
                    FROM v0.jury j
                    GROUP BY j.broadcast_id),

     televote_count AS (SELECT t.broadcast_id,
                               COUNT(*) AS televotes
                        FROM v0.televote t
                        GROUP BY t.broadcast_id)

SELECT b.broadcast_date,
       k.contest_year,
       b.contest_stage,
       cc.competitors,
       COALESCE(jc.juries, 0)    AS juries,
       COALESCE(tc.televotes, 0) AS televotes
FROM v0.broadcast b
         JOIN v0.contest k ON k.contest_id = b.parent_contest_id
         JOIN competitor_count cc ON cc.broadcast_id = b.broadcast_id
         LEFT JOIN jury_count jc ON jc.broadcast_id = b.broadcast_id
         LEFT JOIN televote_count tc ON tc.broadcast_id = b.broadcast_id
WHERE k.queryable = 1;

GO
