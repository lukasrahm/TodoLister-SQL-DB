IF EXISTS ( SELECT [name] FROM sys.databases WHERE [name] = 'TODOLister' )
DROP DATABASE TODOLister
GO

CREATE DATABASE TODOLister
GO

USE TODOLister
GO

CREATE TABLE [Task] (
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,			  
	Title TEXT NOT NULL,
	Description TEXT NOT NULL,
); 


