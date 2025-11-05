SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE FUNCTION dbo.udf_get_contest_stage_sort_order(
    @contest_stage NVARCHAR(10)
)
    RETURNS INT
AS

/*
-- =============================================
-- Function:    dbo.udf_get_contest_stage_sort_order
-- Author:      Matt Tantony
-- Create date: 2025-11-05
--
-- Description:
--  Converts a contest stage name into an integer sort order value.
--
-- Input parameter:
--  @contest_stage NVARCHAR(10). Accepts NULL.
--
-- Returns an INT value.
-- =============================================
*/

BEGIN

    RETURN CASE @contest_stage
               WHEN N'SemiFinal1' THEN 0
               WHEN N'SemiFinal2' THEN 1
               WHEN N'GrandFinal' THEN 2
               ELSE -1 END

END

GO
