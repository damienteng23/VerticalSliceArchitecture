IF TYPE_ID('[dbo].[udtt_CandleType]') IS NOT NULL
BEGIN
    -- You must drop the SPs using the type before dropping the type itself
    IF OBJECT_ID('[dbo].[usp_UpsertCandleData]') IS NOT NULL 
        DROP PROCEDURE [dbo].[usp_UpsertCandleData];

    DROP TYPE [dbo].[udtt_CandleType];
END

CREATE TYPE [dbo].[udtt_CandleType] AS TABLE
(
    [Symbol] [nvarchar](20) NOT NULL,
    [Timestamp] [datetime2](7) NOT NULL,
    [Open] [decimal](18, 4) NOT NULL,
    [High] [decimal](18, 4) NOT NULL,
    [Low] [decimal](18, 4) NOT NULL,
    [Close] [decimal](18, 4) NOT NULL,
    [Volume] [bigint] NOT NULL,
    [AdjustedClose] [decimal](18, 4) NOT NULL
);