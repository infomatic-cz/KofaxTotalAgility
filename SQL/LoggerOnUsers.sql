-- Returns logged on users
SELECT u.[RESOURCE_ID],
    r.NT_NAME,
    LogonType =case
        when [LOGON_PROTOCOL] = 7 then 'WorkSpace'
        when [LOGON_PROTOCOL] = 5 then 'Designer'
    end,
    [LOGGED_ON_TIME],
    [LAST_ACCESSED_TIME]
FROM [dbo].[AW_LOGGEDON_USER] u
    left join NT_RESOURCE r on u.RESOURCE_ID = r.RESOURCE_ID