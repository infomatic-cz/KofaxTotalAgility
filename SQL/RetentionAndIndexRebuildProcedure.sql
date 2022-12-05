SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ondřej Musil
-- Create date: 2022-12-01
-- Description:	Retention policy execution for and optional index rebuild
-- =============================================
CREATE PROCEDURE [dbo].[RetentionAndRebuildIndex]
	@RetentionInDays int
    ,@RebuildIndexes bit
    ,@ProcessingLog NVARCHAR(max) OUT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- setup rollbacku on error, transaction and lock
    set @ProcessingLog = FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss.fff') + ' Creating transaction and lock' + CHAR(13)+CHAR(10)
	set xact_abort on
	BEGIN tran
    EXEC sp_getapplock @Resource = 'RetentionProcessing', @LockMode = 'Exclusive';

        -- label to create loop
        TryDeleteRecords:

            set @ProcessingLog = @ProcessingLog + FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss.fff') + ' Delete temp table' + CHAR(13)+CHAR(10)
            -- delete temp table
            IF object_id('[tempdb]..#GuidsToDelete') is not null
            BEGIN
                exec ('drop table #GuidsToDelete')
            END

            -- save identificator to delete from tables, select only top 100 so delete does not take long
            set @ProcessingLog = @ProcessingLog + FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss.fff') + ' Save GUIDs to delete' + CHAR(13)+CHAR(10)
            
            select TOP (100) GUID 
                INTO #GuidsToDelete
                from [dbo].[main_table]
                where DATEDIFF(day,[CreatedAt],getdate()) > @RetentionInDays

            set @ProcessingLog = CONCAT(@ProcessingLog, FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss.fff'), ' ', @@ROWCOUNT, ' GUIDs found' + CHAR(13)+CHAR(10))

            -- if something was selected, delete from tables
            IF (select count(*) from #GuidsToDelete)>0
            BEGIN

                set @ProcessingLog = @ProcessingLog + FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss.fff') + ' Deleting from tables based on GUIDs' + CHAR(13)+CHAR(10)

                -- add tables that should be cleated
                DELETE FROM [dbo].[main_table]
                    WHERE GUID in (SELECT GUID from #GuidsToDelete)

                DELETE FROM [dbo].[additional_table1]
                    WHERE GUID in (SELECT GUID from #GuidsToDelete)

            END 

            set @ProcessingLog = @ProcessingLog + FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss.fff') + ' Iteration concluded' + CHAR(13)+CHAR(10)
        -- if there is something in table, return to label
        IF (select count(*) from #GuidsToDelete)>0 GOTO TryDeleteRecords

        -- delete temp table
        set @ProcessingLog = @ProcessingLog + FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss.fff') + ' Final delete temp table' + CHAR(13)+CHAR(10)
        IF object_id('[tempdb]..#GuidsToDelete') IS NOT NULL
        BEGIN
            exec ('drop table #GuidsToDelete')
        END

    -- clear lock, commit transaction
    set @ProcessingLog = @ProcessingLog + FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss.fff') + ' Deleting lock, commiting transaction' + CHAR(13)+CHAR(10)
    EXEC sp_releaseapplock @Resource = 'RetentionProcessing';
	COMMIT TRAN

    -- rebuild if enabled
    -- rebuild can take long time, if procedure crashed on timeout, it is probably rebuild
    -- procedure can be called from C# node with custom timeout
    IF @RebuildIndexes = 1
    BEGIN

        BEGIN tran
            EXEC sp_getapplock @Resource = 'RetentionProcessing_IndexRebuild', @LockMode = 'Exclusive';

            set @ProcessingLog = @ProcessingLog + FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss.fff') + ' Rebuilding indexes on main_table' + CHAR(13)+CHAR(10)
            ALTER INDEX ALL ON [dbo].[main_table] REBUILD
            set @ProcessingLog = @ProcessingLog + FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss.fff') + ' Rebuilding completed' + CHAR(13)+CHAR(10)

            set @ProcessingLog = @ProcessingLog + FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss.fff') + ' Rebuilding indexes on additional_table1' + CHAR(13)+CHAR(10)
            ALTER INDEX ALL ON [dbo].[additional_table1] REBUILD
            set @ProcessingLog = @ProcessingLog + FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss.fff') + ' Rebuilding completed' + CHAR(13)+CHAR(10)


            EXEC sp_releaseapplock @Resource = 'RetentionProcessing_IndexRebuild';
    	COMMIT TRAN

    END

    set @ProcessingLog = @ProcessingLog + FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss.fff') + ' Processing concluded' + CHAR(13)+CHAR(10)

END
GO