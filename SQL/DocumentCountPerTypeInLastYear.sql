-- Number of created documents per type in last year
-- KTA version after Documents DB change (7.8 and higher)
SELECT [TypeName], Počet=count(1)
from [dbo].[DocumentData]
where [CreatedAt] between dateadd(YEAR, -1,getdate()) and getdate()
group by [TypeName]
order by 2 desc


-- Number of created documents per type in last year
-- KTA version before Documents DB change (<=7.7 and lower)
SELECT [DisplayName], Počet=count(1)
from [dbo].[Document]
where [CreatedAt] between dateadd(YEAR, -1,getdate()) and getdate()
group by [DisplayName]
order by 2 desc
