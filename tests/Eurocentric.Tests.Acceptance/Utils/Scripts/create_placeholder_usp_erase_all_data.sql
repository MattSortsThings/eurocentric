SET ANSI_NULLS ON;


GO
SET QUOTED_IDENTIFIER ON;


GO
CREATE PROCEDURE [placeholder].[usp_erase_all_data]
AS
BEGIN
    SET NOCOUNT ON;
    
    TRUNCATE TABLE [placeholder].[broadcast_competitor_points_award];
    
    TRUNCATE TABLE [placeholder].[broadcast_jury];
    
    TRUNCATE TABLE [placeholder].[broadcast_televote];
    
    DELETE [placeholder].[broadcast]
    WHERE  [broadcast_id] <> N'00000000-0000-0000-0000-000000000000';
    
    TRUNCATE TABLE [placeholder].[contest_broadcast_memo];
    
    TRUNCATE TABLE [placeholder].[contest_participant];
    
    DELETE [placeholder].[contest]
    WHERE  [contest_id] <> N'00000000-0000-0000-0000-000000000000';
    
    TRUNCATE TABLE [placeholder].[country];
END
