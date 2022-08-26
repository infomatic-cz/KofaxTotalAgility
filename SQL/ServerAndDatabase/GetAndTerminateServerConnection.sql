-- In case of DB operations that require no active connections it is possible to list and terminate connections

-- List connections, for more details use sp_who2
-- Require VIEW SERVER STATE permission
exec sp_who

-- Kill specific connetion by id (xxx = spid from previous result)
KILL xxx
