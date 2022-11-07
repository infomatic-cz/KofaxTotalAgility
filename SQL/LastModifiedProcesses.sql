
-- Returns last modified process with few details

SELECT TOP (1000)
	  [Category] = c.NAME
	  ,[PROCESS_NAME]
	  --,[PROCESS_ID]
      ,[VERSION]
	  ,[CHANGE_DATE]
	  ,[LastModifiedBy] = r.NT_NAME
  FROM [dbo].[BUSINESS_PROCESS] bp
	left join [dbo].[CATEGORY] c on c.[CATEGORY_ID] = bp.[CATEGORY_ID]
	left join [dbo].NT_RESOURCE r on bp.[LAST_MODIFY_RESOURCE] = r.[RESOURCE_ID]

  where [LATEST_VERSION] = 1

  order by [CHANGE_DATE] desc