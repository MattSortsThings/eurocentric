SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE FUNCTION dbo.udf_get_contest_stage_sort_order_value(
    @contest_stage NVARCHAR(10) = NULL
)
    RETURNS INT
AS

/*
-- =============================================
-- Function:    dbo.udf_get_contest_stage_sort_order_value
-- Author:      Matt Tantony
-- Create date: 2025-11-09
--
-- Description:
--  Converts a 'ContestStage' enum value to its non-negative integer sort order value. Returns -1 if the input parameter
--  is NULL or is not a value from the 'ContestStage' enum.
--
-- Input parameter:
--  @contest_stage NVARCHAR(10)
--      - A value from the 'ContestStage' enum.
--      - Valid values: {'SemiFinal1', 'SemiFinal2', 'GrandFinal'}.
--      - Allows invalid values.
--      - Allows NULL. Default value is NULL.
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
