SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE FUNCTION dbo.udf_normalize_points_value(
    @points_value INT,
    @max_points_value INT
)
    RETURNS FLOAT
AS

/*
-- =============================================
-- Function:    dbo.udf_normalize_points_value
-- Author:      Matt Tantony
-- Create date: 2025-11-09
--
-- Description:
--  Normalizes a points value to a floating point number in the range [1.0, 10.0].
--
-- Input parameters:
--  @points_value INT
--      - A non-negative integer. The points value to be normalized.
--  @max_points_value INT
--      - An integer greater than 0. The maximum points value.
--
-- Returns a FLOAT value in the range [1.0, 10.0].
-- =============================================
*/

BEGIN

    RETURN CASE
               WHEN @points_value < 1 THEN 1.0
               WHEN @points_value >= @max_points_value THEN 10.0
               ELSE 1.0 + ((9.0 * @points_value) / @max_points_value) END

END

GO
