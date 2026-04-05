CREATE PROCEDURE [dbo].[sp_UpsertDailyTickers]
    @DailyTickers [dbo].[udtt_DailyTickerType] READONLY
AS
BEGIN
    SET NOCOUNT ON;

    MERGE INTO [dbo].[DailyTickerSummary] AS target
    USING @DailyTickers AS source
    ON target.[Symbol] = source.[Symbol] 
    AND target.[Date] = source.[Date]
    
    WHEN MATCHED THEN
        UPDATE SET
            target.[Open] = source.[Open],
            target.[High] = source.[High],
            target.[Low] = source.[Low],
            target.[Close] = source.[Close],
            target.[Volume] = source.[Volume],
            target.[PreMarket] = source.[PreMarket],
            target.[AfterHours] = source.[AfterHours],
            target.[Status] = source.[Status],
            target.[UpdatedAt] = GETUTCDATE()
    
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([Symbol], [Date], [Open], [High], [Low], [Close], [Volume], [PreMarket], [AfterHours], [Status])
        VALUES (source.[Symbol], source.[Date], source.[Open], source.[High], source.[Low], 
                source.[Close], source.[Volume], source.[PreMarket], source.[AfterHours], source.[Status]);
END