-- Migrate database audit columns

-- EnforcementOrders

update t
set CreatedAt = CreatedDate at time zone 'Eastern Standard Time',
    UpdatedAt = UpdatedDate at time zone 'Eastern Standard Time'
from EnforcementOrders t;

update t
set t.CreatedBy = u.EmailAddress
from EnforcementOrders t
    inner join EnfoUsers u
    on t.CreatedById = u.Id;

update t
set t.UpdatedBy = u.EmailAddress
from EnforcementOrders t
    inner join EnfoUsers u
    on t.UpdatedById = u.Id;
go

alter table EnforcementOrders
    drop column CreatedDate, UpdatedDate, CreatedById, UpdatedById;
go

-- EpdContacts

update t
set CreatedAt = CreatedDate at time zone 'Eastern Standard Time',
    UpdatedAt = UpdatedDate at time zone 'Eastern Standard Time'
from EpdContacts t;

update t
set t.CreatedBy = u.EmailAddress
from EpdContacts t
    inner join EnfoUsers u
    on t.CreatedById = u.Id;

update t
set t.UpdatedBy = u.EmailAddress
from EpdContacts t
    inner join EnfoUsers u
    on t.UpdatedById = u.Id;
go

alter table EpdContacts
    drop column CreatedDate, UpdatedDate, CreatedById, UpdatedById;
go

-- LegalAuthorities

update t
set CreatedAt = CreatedDate at time zone 'Eastern Standard Time',
    UpdatedAt = UpdatedDate at time zone 'Eastern Standard Time'
from LegalAuthorities t;

update t
set t.CreatedBy = u.EmailAddress
from LegalAuthorities t
    inner join EnfoUsers u
    on t.CreatedById = u.Id;

update t
set t.UpdatedBy = u.EmailAddress
from LegalAuthorities t
    inner join EnfoUsers u
    on t.UpdatedById = u.Id;
go

alter table LegalAuthorities
    drop column CreatedDate, UpdatedDate, CreatedById, UpdatedById;
go

-- Update audit data where user data was not originally available

update EnforcementOrders
set CreatedBy = case
                    when CreatedBy = '0@dnr.ga.gov' then 'kfrazier'
                    when CreatedBy = '00@dnr.ga.gov' then 'johnson'
                    when CreatedBy = '000@dnr.ga.gov' then 'lewis'
                    when CreatedBy = '0000@dnr.ga.gov' then 'lhughes'
    end,
    UpdatedBy = case
                    when UpdatedBy = '0@dnr.ga.gov' then 'kfrazier'
                    when UpdatedBy = '00@dnr.ga.gov' then 'johnson'
                    when UpdatedBy = '000@dnr.ga.gov' then 'lewis'
                    when UpdatedBy = '0000@dnr.ga.gov' then 'lhughes'
        end
where CreatedBy like '0%';

go
