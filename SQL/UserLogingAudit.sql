-- Returns 1000 last audit entries related to user login or logoff
-- Can be filtered to specific user by populating @UserName variable with part or full username
DECLARE @UserName nvarchar(50) = ''
SELECT TOP 1000 AUDIT_TIME,
    r.NT_NAME,
    AUDIT_ENTRY_DESCRIPTION,
    a.RESOURCE_ID
FROM dbo.AUDIT_LOG a
    INNER JOIN dbo.NT_RESOURCE r ON a.RESOURCE_ID = r.RESOURCE_ID
    AND r.NT_NAME LIKE '%' + isnull(nullif(@UserName, ''), r.NT_NAME) + '%'
WHERE AUDIT_TYPE IN (
        SELECT AUDIT_TYPE
        FROM dbo.AUDIT_TYPE
        WHERE AUDIT_CATEGORY = 8
    )
ORDER BY AUDIT_TIME desc