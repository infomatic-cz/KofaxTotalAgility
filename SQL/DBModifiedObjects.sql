-- Returns last 100 modified object in database
-- Modification is change in table structure, modification of stored procedure etc.
-- Modification is not insert/update/delete in table
-- Ordered by modification date, descending (newest modification first)
select top 100 name, type_desc, create_date, modify_date 
from sys.objects
--where modify_date>'2020-10-01'	-- uncomment and set date for date filter
where is_ms_shipped = 0
order by modify_date desc
