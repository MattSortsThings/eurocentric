SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE VIEW v0.vw_queryable_contest
AS

/*
-- =============================================
-- View:        v0.vw_queryable_contest
-- Author:      Matt Tantony
-- Create date: 2025-09-25
--
-- Description:
--  Comprises a summary record for every queryable contest in the v0 schema.
--
-- Records have the following columns:
--  - contest_year INT
--  - city_name NVARCHAR(200)
--  - participants INT
--  - uses_rest_of_world_televote BIT
-- =============================================
*/

SELECT k.contest_year,
       k.city_name,
       cp.participants,
       CAST(IIF(k.global_televote_voting_country_id IS NULL, 0, 1) AS BIT) AS uses_rest_of_world_televote
FROM (SELECT p.contest_id,
             COUNT(*) AS participants
      FROM v0.participant p
      GROUP BY p.contest_id) cp
         JOIN v0.contest k ON k.contest_id = cp.contest_id
WHERE k.queryable = 1;

GO
