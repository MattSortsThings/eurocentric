SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE VIEW v0.vw_queryable_country
AS

/*
-- =============================================
-- View:        v0.vw_queryable_country
-- Author:      Matt Tantony
-- Create date: 2025-09-25
--
-- Description:
--  Comprises a summary record for every queryable country in the v0 schema.
--
-- Records have the following columns:
--  - country_code NCHAR(2)
--  - country_name NVARCHAR(200)
-- =============================================
*/

SELECT DISTINCT c.country_code,
                c.country_name
FROM v0.country c
         JOIN v0.contest_role cr ON cr.country_id = c.country_id
         JOIN v0.contest k ON k.contest_id = cr.contest_id
WHERE k.queryable = 1;

GO
