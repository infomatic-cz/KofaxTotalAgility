-- Returns 100 longest jobs created last day with name and length of longest activity
-- Works best with process name filter on synchronous processes (longest activity in asynchronous processes is usualy manual activity)
-- Created to find longest runing postvalidation handlers
with CTE as (
select top (100) 
	[JOB_ID]
	,[START_TIME]
	,[FINISH_TIME]
	,[PROCESS_ID]
    ,[PROCESS_NAME]
	,[VERSION]
	,Duration = datediff(s,[START_TIME],[FINISH_TIME])
	-- ,Duration = datediff(ms,[START_TIME],[FINISH_TIME])	-- změna přesnosti na milisekundy, odkomentovat tento řádek, zakomentovat předchozí
	FROM [dbo].[FINISHED_JOB] j
	where --[PROCESS_NAME] = '' and		-- uncomment and add process name here
		[FINISH_TIME] > [START_TIME] and
		[START_TIME] between DATEADD(day,-1,getdate()) and DATEADD(day,1,getdate())	-- adding one day to now in case of different time zones between SQL server and client
		-- and [VERSION] = 1	-- uncomment for specific version
	order by Duration desc
)
select CTE.*
	,LongestActivityName = (select top 1 [NODE_NAME] FROM [dbo].[FINISHED_JOB_HISTORY] where [JOB_ID] = CTE.JOB_ID order by [WORKING_TIME_SPENT_IN_SECONDS] desc)
	,LongestActivityDuration = (select top 1 [TIME_SPENT_IN_SECONDS] FROM [dbo].[FINISHED_JOB_HISTORY] where [JOB_ID] = CTE.JOB_ID order by [WORKING_TIME_SPENT_IN_SECONDS] desc)
from CTE
order by Duration desc
