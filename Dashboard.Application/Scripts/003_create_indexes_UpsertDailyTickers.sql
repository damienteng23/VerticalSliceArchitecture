IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_DailyTickerSummary_Symbol_From' 
    AND object_id = OBJECT_ID('[dbo].[DailyTickerSummary]')
)
CREATE NONCLUSTERED INDEX [IX_DailyTickerSummary_Symbol_From] 
    ON [dbo].[DailyTickerSummary]([Symbol], [From]);
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_DailyTickerSummary_From' 
    AND object_id = OBJECT_ID('[dbo].[DailyTickerSummary]')
)
CREATE NONCLUSTERED INDEX [IX_DailyTickerSummary_From] 
    ON [dbo].[DailyTickerSummary]([From]);
GO