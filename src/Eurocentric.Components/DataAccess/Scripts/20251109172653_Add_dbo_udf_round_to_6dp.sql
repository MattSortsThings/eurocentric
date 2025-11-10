SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE FUNCTION dbo.udf_round_to_6dp(
    @value FLOAT
)
    RETURNS DECIMAL(18, 6)
AS

/*
-- =============================================
-- Function:    dbo.udf_round_to_6dp
-- Author:      Matt Tantony
-- Create date: 2025-11-09
--
-- Description:
--  Rounds a floating point value to 6 decimal places and returns the result as a decimal value.
--
-- Input parameter:
--  @value FLOAT
--      - The floating point value to be rounded.
--
-- Returns a DECIMAL(18,6) value.
-- =============================================
*/

BEGIN

    RETURN CAST(ROUND(@value, 6) AS DECIMAL(18, 6));

END

GO
