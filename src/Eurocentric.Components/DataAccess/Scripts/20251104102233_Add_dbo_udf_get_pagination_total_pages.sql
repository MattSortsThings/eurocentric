SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE FUNCTION dbo.udf_get_pagination_total_pages(
    @page_size INT,
    @total_items INT
)
    RETURNS INT
AS

/*
-- =============================================
-- Function:    dbo.udf_get_pagination_total_pages
-- Author:      Matt Tantony
-- Create date: 2025-11-04
--
-- Description:
--  Calculates the total number of pages given the pagination page size and total items.
--
-- Input parameter:
--  @page_size INT
--  @total_items INT
--
-- Returns an INT value.
-- =============================================
*/

BEGIN

RETURN IIF(@total_items = 0, 0, CEILING((1.0 * @total_items) / @page_size));

END

GO
