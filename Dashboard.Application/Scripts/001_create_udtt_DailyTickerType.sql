IF TYPE_ID('[dbo].[udtt_DailyTickerType]') IS NOT NULL
BEGIN
    -- Drop SPs that use the type first
    IF OBJECT_ID('[dbo].[sp_UpsertDailyTickers]') IS NOT NULL 
        DROP PROCEDURE [dbo].[sp_UpsertDailyTickers];

    DROP TYPE [dbo].[udtt_DailyTickerType];
END

CREATE TYPE [dbo].[udtt_DailyTickerType] AS TABLE
(
    [Symbol] [nvarchar](10) NOT NULL,
    [Date] [datetime2](7) NOT NULL,
    [Open] [decimal](18, 4) NOT NULL,
    [High] [decimal](18, 4) NOT NULL,
    [Low] [decimal](18, 4) NOT NULL,
    [Close] [decimal](18, 4) NOT NULL,
    [Volume] [bigint] NOT NULL,
    [PreMarket] [decimal](18, 4) NULL,
    [AfterHours] [decimal](18, 4) NULL,
    [Status] [nvarchar](50) NULL
);