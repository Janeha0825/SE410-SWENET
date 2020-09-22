-- Create database
use master 
go

create database dotwiki
go

-- Create user
use master 
go

exec sp_addlogin 'dotwikiuser', 'dotwikipwd', 'dotwiki'
go

-- Grant user access to database
use dotwiki
go

exec sp_grantdbaccess 'dotwikiuser'
go

exec sp_addrolemember 'db_owner', 'dotwikiuser'
go

use master 
go
