-- Returns size and name of all database files (including log, temp etc.)
-- Ordered by size, descending
SELECT [Id]=database_id
    ,[Name]=DB_NAME(database_id)
    ,[Type]=type_desc
    ,[FileName]=name 
    ,[FileSizeGB]=FORMAT((cast(size as float)*8)/1024/1024, 'N')
    ,[FilePath]=physical_name
FROM sys.master_files
order by size desc
