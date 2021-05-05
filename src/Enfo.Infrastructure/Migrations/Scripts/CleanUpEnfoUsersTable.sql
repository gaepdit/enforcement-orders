-- Clean up and deprecate `EnfoUsers` table

alter table EnfoUsers drop column LastPasswordChangedDate
alter table EnfoUsers drop column PasswordHash
alter table EnfoUsers drop column RequirePasswordChange
alter table EnfoUsers drop column UpdatedDate
go

exec sp_addextendedproperty 'MS_Description', 'Deprecated', 'SCHEMA', 'dbo', 'TABLE', 'EnfoUsers'
go
