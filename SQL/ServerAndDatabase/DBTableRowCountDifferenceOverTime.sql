
-- Returns table row count difference over specified delay
-- Table row count for all tables is saved after start and after specified delay. Difference is simple subtraction
-- In case same number of rows was added and removed during delay, difference will be 0, there is not identification for unique rows
-- Created to simply check if Job Cleardown is still runing. It will timeout but there is not error message in Designer
--	so it seems like it is still runing but it is not and no jobs are being removed
-- Delay is defined variable @delay below. Format is hh:mm:ss, 00:00:10 is 10 seconds
DECLARE @delay NVARCHAR(50) = '00:00:10'

-- Delete temp tables
IF object_id('[tempdb]..'+ '#Temp1') is not null
BEGIN
    EXEC ('drop table ' + '#Temp1')
END

IF object_id('[tempdb]..'+ '#Temp2') is not null
BEGIN
    EXEC ('drop table ' + '#Temp2')
END

-- Save initial row counts
SELECT
      QUOTENAME(SCHEMA_NAME(sOBJ.schema_id)) + '.' + QUOTENAME(sOBJ.name) AS [TableName]
      ,[RowCount] = SUM(sPTN.Rows)
INTO #Temp1
FROM 
      sys.objects AS sOBJ
      INNER JOIN sys.partitions AS sPTN
            ON sOBJ.object_id = sPTN.object_id
WHERE
      sOBJ.type = 'U'
      AND sOBJ.is_ms_shipped = 0x0
      AND index_id < 2 -- 0:Heap, 1:Clustered
GROUP BY 
      sOBJ.schema_id
      , sOBJ.name
ORDER BY [RowCount] desc-- [TableName]

-- Wait defined delay
WAITFOR DELAY @delay;

-- Save row count after delay
SELECT
      QUOTENAME(SCHEMA_NAME(sOBJ.schema_id)) + '.' + QUOTENAME(sOBJ.name) AS [TableName]
      ,[RowCount] = SUM(sPTN.Rows)
INTO #Temp2
FROM 
      sys.objects AS sOBJ
      INNER JOIN sys.partitions AS sPTN
            ON sOBJ.object_id = sPTN.object_id
WHERE
      sOBJ.type = 'U'
      AND sOBJ.is_ms_shipped = 0x0
      AND index_id < 2 -- 0:Heap, 1:Clustered
GROUP BY 
      sOBJ.schema_id
      , sOBJ.name
ORDER BY [RowCount] desc-- [TableName]

-- Compare row counts
SELECT t1.TableName, RowCountDifference = t2.[RowCount]-t1.[RowCount]
FROM #Temp1 t1
	inner join #Temp2 t2 ON t1.TableName = t2.TableName and t1.[RowCount] != t2.[RowCount]
ORDER BY 2

-- Delete temp tables
if object_id('[tempdb]..'+ '#Temp1') is not null
begin
    exec ('drop table ' + '#Temp1')
end

if object_id('[tempdb]..'+ '#Temp2') is not null
begin
    exec ('drop table ' + '#Temp2')
end
