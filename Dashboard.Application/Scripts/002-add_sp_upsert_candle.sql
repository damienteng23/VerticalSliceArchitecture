CREATE OR ALTER PROCEDURE [dbo].[usp_UpsertCandleData]
    @CandleData [dbo].[udtt_CandleType] READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        MERGE [dbo].[Candle] WITH (HOLDLOCK) AS Target
        USING @CandleData AS Source
        ON (Target.[Symbol] = Source.[Symbol] AND Target.[Timestamp] = Source.[Timestamp])
        
        -- 1. If a match is found, update the existing candle values
        WHEN MATCHED THEN
            UPDATE SET 
                Target.[Open] = Source.[Open],
                Target.[High] = Source.[High],
                Target.[Low] = Source.[Low],
                Target.[Close] = Source.[Close],
                Target.[Volume] = Source.[Volume],
                Target.[AdjustedClose] = Source.[AdjustedClose]

        -- 2. If no match is found, insert the new candle
        WHEN NOT MATCHED THEN
            INSERT ([Symbol], [Timestamp], [Open], [High], [Low], [Close], [Volume], [AdjustedClose])
            VALUES (Source.[Symbol], Source.[Timestamp], Source.[Open], Source.[High], Source.[Low], Source.[Close], Source.[Volume], Source.[AdjustedClose]);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF (XACT_STATE()) <> 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END