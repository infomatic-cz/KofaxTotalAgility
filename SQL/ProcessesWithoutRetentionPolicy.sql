-- Returns processes without retention policy
-- Works on KTA 7.10 and lower
;with CTE as(
  SELECT
    Category = c.NAME
    --[PROCESS_ID]
    ,[PROCESS_NAME]
    ,[VERSION]
    ,[LATEST_VERSION]
    ,KeepForever=[SETTINGS_XML].value('(BusinessProcessSettings/RetentionPolicySettings/KeepForever)[1]', 'bit')
    ,Years=[SETTINGS_XML].value('(BusinessProcessSettings/RetentionPolicySettings/RetentionPolicyDuration/Years)[1]', 'int')
    ,Months=[SETTINGS_XML].value('(BusinessProcessSettings/RetentionPolicySettings/RetentionPolicyDuration/Months)[1]', 'int')
    ,Days=[SETTINGS_XML].value('(BusinessProcessSettings/RetentionPolicySettings/RetentionPolicyDuration/Days)[1]', 'int')
    ,Hours=[SETTINGS_XML].value('(BusinessProcessSettings/RetentionPolicySettings/RetentionPolicyDuration/Hours)[1]', 'int')
    ,Minutes=[SETTINGS_XML].value('(BusinessProcessSettings/RetentionPolicySettings/RetentionPolicyDuration/Minutes)[1]', 'int')
    --,[CATEGORY_ID]
    --,[CHANGE_DATE]
    --,[CREATION_DATE]
    --,[CREATOR]
    ,[PROCESS_TYPE]
    --,[SETTINGS_XML]
  FROM [dbo].[BUSINESS_PROCESS] bp
  left join [dbo].[CATEGORY] c on c.CATEGORY_ID = bp.CATEGORY_ID
)
select * 
  from CTE
  where KeepForever = 1 and [LATEST_VERSION] = 1 
    and [PROCESS_TYPE] != 5 -- exclude business rules
  order by 1,2
