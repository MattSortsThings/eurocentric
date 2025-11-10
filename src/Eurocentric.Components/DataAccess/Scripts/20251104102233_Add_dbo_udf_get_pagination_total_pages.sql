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
-- Input parameters:
--  @page_size INT
--      - An integer greater than 0. The number of items per page.
--  @total_items INT
--      - A non-negative integer. The total number of items before pagination.
--
-- Returns a non-negative INT value.
-- =============================================
*/

BEGIN

    RETURN IIF(@total_items = 0, 0, CEILING((1.0 * @total_items) / @page_size));

END

GO
