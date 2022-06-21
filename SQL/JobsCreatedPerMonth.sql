-- Returns number of jobs created per month for specified number of years
-- Also returns total for each year and total for all years
-- Works on live and finished jobs
-- Number of years can be modified in @YearsToPast (set negative value )
-- Can be filtered to specific process by setting @ProcessName
DECLARE @YearsToPast int = -3;
DECLARE @ProcessName nvarchar(50) = '';
WITH Jobs AS (
    SELECT [JOB_ID],
        [CREATION_TIME]
    FROM [dbo].[JOB]
    WHERE [CREATION_TIME] BETWEEN DATEADD(YEAR, @YearsToPast, GETDATE()) and DATEADD(DAY, 1, GETDATE())
        AND PROCESS_NAME = ISNULL(NULLIF(@ProcessName, ''), [PROCESS_NAME])
    UNION ALL
    SELECT [JOB_ID],
        [CREATION_TIME]
    FROM [dbo].[FINISHED_JOB]
    WHERE [CREATION_TIME] BETWEEN DATEADD(YEAR, @YearsToPast, GETDATE()) and DATEADD(DAY, 1, GETDATE())
        AND PROCESS_NAME = ISNULL(NULLIF(@ProcessName, ''), [PROCESS_NAME])
)
SELECT [Year] = ISNULL(
        CAST(YEAR([CREATION_TIME]) AS nvarchar),
        'GrandTotal'
    ),
    [Month] = ISNULL(
        CAST(MONTH([CREATION_TIME]) AS nvarchar),
        'Total'
    ),
    [JobCount] = COUNT([JOB_ID])
FROM Jobs
GROUP BY ROLLUP(YEAR([CREATION_TIME]), MONTH([CREATION_TIME]))
ORDER BY YEAR([CREATION_TIME]) DESC,
    MONTH([CREATION_TIME]) DESC