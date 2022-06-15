-- Returns version of MS SQL, server name and instance name
-- Simple "Where am I?"
select SQLVersion=@@VERSION,ServerName=@@SERVERNAME,Instance=@@servicename
