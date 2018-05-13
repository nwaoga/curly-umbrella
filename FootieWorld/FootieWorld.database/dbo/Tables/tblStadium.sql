CREATE TABLE [dbo].[tblStadium]
(
	[StadiumID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(255) NULL, 
    [Address1] VARCHAR(255) NULL, 
    [Address2] VARCHAR(255) NULL, 
    [Address3] VARCHAR(255) NULL, 
    [Address4] VARCHAR(255) NULL, 
    [Capacity] INT NULL, 
    [Postcode] NCHAR(10) NULL 
)
