-- Trim all text fields and remove tab and new line characters

update EnforcementOrders
set Cause = trim(replace(replace(replace(Cause, char(12), ' '), char(10), ' '), char(09), ' '))
where Cause like '%' + char(09) + '%'
   or Cause like '%' + char(10) + '%'
   or Cause like '%' + char(12) + '%';

update EnforcementOrders
set FacilityName = trim(replace(replace(replace(FacilityName, char(12), ' '), char(10), ' '), char(09), ' '))
where FacilityName like '%' + char(09) + '%'
   or FacilityName like '%' + char(10) + '%'
   or FacilityName like '%' + char(12) + '%';

update EnforcementOrders
set Requirements = trim(replace(replace(replace(Requirements, char(12), ' '), char(10), ' '), char(09), ' '))
where Requirements like '%' + char(09) + '%'
   or Requirements like '%' + char(10) + '%'
   or Requirements like '%' + char(12) + '%';

go
