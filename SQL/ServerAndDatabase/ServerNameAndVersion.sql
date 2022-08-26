-- Returns version of MS SQL, server name and instance name
-- Simple "Where am I?"
select SQLVersion=@@VERSION,ServerName=@@SERVERNAME,Instance=@@servicename

-- List of databases with basic information
-- Details about compatibility level https://docs.microsoft.com/en-us/sql/t-sql/statements/alter-database-transact-sql-compatibility-level
SELECT
	database_id
	,name
	,compatibility_level
	,collation_name
	,state_desc
	,recovery_model_desc
FROM sys.databases
