-- Addresses

update t
set CreatedAt = CreatedDate at time zone 'Eastern Standard Time',
    UpdatedAt = UpdatedDate at time zone 'Eastern Standard Time'
from Addresses t;

update t
set t.CreatedBy = u.EmailAddress
from Addresses t
    inner join EnfoUsers u
    on t.CreatedById = u.Id;

update t
set t.UpdatedBy = u.EmailAddress
from Addresses t
    inner join EnfoUsers u
    on t.UpdatedById = u.Id;

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

-- Drop obsolete columns

alter table Addresses
    drop column CreatedDate, UpdatedDate, CreatedById, UpdatedById;
alter table EnforcementOrders
    drop column CreatedDate, UpdatedDate, CreatedById, UpdatedById;
alter table EpdContacts
    drop column CreatedDate, UpdatedDate, CreatedById, UpdatedById;
alter table LegalAuthorities
    drop column CreatedDate, UpdatedDate, CreatedById, UpdatedById;

