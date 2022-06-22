-- Returns number of jobs created per year, month, day and hour
-- Also returns total job count for each year, month, day and total for all years
-- Works on live and finished jobs
-- Date range can be specified by interval from today or by specific dates
-- When using in unknown environment start with shorter periods and check how long query is runing

-- ********** Configuration
-- Can be filtered to specific process by setting @ProcessName
DECLARE @ProcessName nvarchar(50) = '';

-- Set granularity of output
DECLARE @Outputs nvarchar(1) = 'h'; -- possible values: y m d h

-- Set interval to past from today
DECLARE @IntervalType nvarchar(1) = 'd';   -- possible values: y m d h
DECLARE @IntervalSize int = 7;

-- Or specify start and end date
DECLARE @StartDate date = null;     -- use format yyyy-MM-dd -> '2022-01-20'
DECLARE @EndDate date = null;

-- ********** Logic, do not touch unless you know what you are doing
-- Simple input check, might use some tinkering if i have time
IF ((@StartDate is not null  OR @EndDate is not null) and (LEN(@IntervalType) > 0 OR @IntervalSize <> 0)) THROW 50000,'Provide interval or dates, not both',1;
IF ((@StartDate is null OR @EndDate is null) and (LEN(@IntervalType) = 0 OR @IntervalSize = 0 OR @IntervalSize is null)) THROW 50000,'Provide interval or dates',1;

-- Setup variables
SET @ProcessName = NULLIF(@ProcessName, '');
IF (@StartDate is null and @EndDate is null) 
BEGIN 
    set @IntervalSize = ABS(@IntervalSize) * -1
    IF (@IntervalType = 'h') SET @StartDate = DATEADD(hour, @IntervalSize, GETDATE()) 
    IF (@IntervalType = 'd') SET @StartDate = DATEADD(day, @IntervalSize, GETDATE()) 
    IF (@IntervalType = 'm') SET @StartDate = DATEADD(month, @IntervalSize, GETDATE()) 
    IF (@IntervalType = 'y') SET @StartDate = DATEADD(year, @IntervalSize, GETDATE())
    SET @EndDate = DATEADD(day, 1, GETDATE())
END;

if object_id('[tempdb]..#TempJobs') is not null drop table #TempJobs;

-- Put together jobs and finished jobs
Select * into #TempJobs 
from (
        SELECT [JOB_ID],
            [CREATION_TIME]
        FROM [dbo].[JOB]
        WHERE [CREATION_TIME] BETWEEN @StartDate and @EndDate
            AND PROCESS_NAME = ISNULL(@ProcessName, [PROCESS_NAME])
        UNION ALL
        SELECT [JOB_ID],
            [CREATION_TIME]
        FROM [dbo].[FINISHED_JOB]
        WHERE [CREATION_TIME] BETWEEN @StartDate and @EndDate
            AND PROCESS_NAME = ISNULL(@ProcessName, [PROCESS_NAME])
    ) j;

-- Hour
if charindex('h', @Outputs) > 0
SELECT [Year] = ISNULL(
        CAST(YEAR([CREATION_TIME]) AS nvarchar),
        'Total'
    ),
    [Month] = ISNULL(
        CAST(MONTH([CREATION_TIME]) AS nvarchar),
        'Total'
    ),
    [Day] = ISNULL(
        CAST(DAY([CREATION_TIME]) AS nvarchar),
        'Total'
    ),
    [Hour] = ISNULL(
        CAST(DATEPART(hour, [CREATION_TIME]) AS nvarchar),
        'Total'
    ),
    [JobCount] = COUNT([JOB_ID])
FROM #TempJobs
GROUP BY ROLLUP(
        YEAR([CREATION_TIME]),
        MONTH([CREATION_TIME]),
        DAY([CREATION_TIME]),
        DATEPART(hour, [CREATION_TIME])
    )
ORDER BY YEAR([CREATION_TIME]) DESC,
    MONTH([CREATION_TIME]) DESC,
    DAY([CREATION_TIME]) DESC,
    DATEPART(hour, [CREATION_TIME]) DESC;

-- Day
if charindex('d', @Outputs) > 0
SELECT [Year] = ISNULL(
        CAST(YEAR([CREATION_TIME]) AS nvarchar),
        'Total'
    ),
    [Month] = ISNULL(
        CAST(MONTH([CREATION_TIME]) AS nvarchar),
        'Total'
    ),
    [Day] = ISNULL(
        CAST(DAY([CREATION_TIME]) AS nvarchar),
        'Total'
    ),
    [JobCount] = COUNT([JOB_ID])
FROM #TempJobs
GROUP BY ROLLUP(
        YEAR([CREATION_TIME]),
        MONTH([CREATION_TIME]),
        DAY([CREATION_TIME])
    )
ORDER BY YEAR([CREATION_TIME]) DESC,
    MONTH([CREATION_TIME]) DESC,
    DAY([CREATION_TIME]) DESC;

-- Month
if charindex('m', @Outputs) > 0
SELECT [Year] = ISNULL(
        CAST(YEAR([CREATION_TIME]) AS nvarchar),
        'Total'
    ),
    [Month] = ISNULL(
        CAST(MONTH([CREATION_TIME]) AS nvarchar),
        'Total'
    ),
    [JobCount] = COUNT([JOB_ID])
FROM #TempJobs
GROUP BY ROLLUP(YEAR([CREATION_TIME]), MONTH([CREATION_TIME]))
ORDER BY YEAR([CREATION_TIME]) DESC,
    MONTH([CREATION_TIME]) DESC;

-- Year
if charindex('y', @Outputs) > 0
SELECT [Year] = ISNULL(
        CAST(YEAR([CREATION_TIME]) AS nvarchar),
        'Total'
    ),
    [JobCount] = COUNT([JOB_ID])
FROM #TempJobs
GROUP BY ROLLUP(YEAR([CREATION_TIME]))
ORDER BY YEAR([CREATION_TIME]) DESC;

if object_id('[tempdb]..#TempJobs') is not null drop table #TempJobs;