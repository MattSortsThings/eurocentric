SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE FUNCTION dbo.tvf_map_contest_stage_filter_enum(
    @contest_stage_filter NVARCHAR(10)
)
    RETURNS @result TABLE
                    (
                        contest_stage NVARCHAR(10)
                    )
AS

/*
-- =============================================
-- Function:    dbo.tvf_map_contest_stage_filter_enum
-- Author:      Matt Tantony
-- Create date: 2025-10-19
--
-- Description:
--  Maps a nullable 'ContestStageFilter' enum value to a table populated with 'ContestStage' enum values.
--
-- Input parameter:
--  @contest_stage_filter NVARCHAR(10)
--      - A value from the 'ContestStageFilter' enum.
--      - Valid values: {'Any', 'SemiFinal1', 'SemiFinal2', 'SemiFinals', 'GrandFinal'}.
--      - Allows NULL. Resolves NULL value to 'Any'.
--
-- Returns a table with the following column:
--  - contest_stage NVARCHAR(10)
--      - Values from the 'ContestStage' enum: {'SemiFinal1', 'SemiFinal2', 'GrandFinal'}.
-- =============================================
*/

BEGIN

    DECLARE @resolved_contest_stage_filter NVARCHAR(10) = COALESCE(@contest_stage_filter, N'Any');

    IF @resolved_contest_stage_filter = N'SemiFinal1'
        BEGIN
            INSERT INTO @result (contest_stage) VALUES (N'SemiFinal1')
        END

    ELSE
        IF @resolved_contest_stage_filter = N'SemiFinal2'
            BEGIN
                INSERT INTO @result (contest_stage) VALUES (N'SemiFinal2')
            END

        ELSE
            IF @resolved_contest_stage_filter = N'SemiFinals'
                BEGIN
                    INSERT INTO @result (contest_stage)
                    VALUES (N'SemiFinal1'),
                           (N'SemiFinal2')
                END

            ELSE
                IF @resolved_contest_stage_filter = N'GrandFinal'
                    BEGIN
                        INSERT INTO @result (contest_stage) VALUES (N'GrandFinal')
                    END

                ELSE
                    BEGIN
                        INSERT INTO @result (contest_stage)
                        VALUES (N'SemiFinal1'),
                               (N'SemiFinal2'),
                               (N'GrandFinal')
                    END
    RETURN

END

GO
