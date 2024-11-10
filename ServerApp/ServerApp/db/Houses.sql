CREATE TABLE [dbo].[Houses]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Owner] INT NOT NULL, 
    [Name] VARCHAR(30) NULL, 
    [GiftType] INT NOT NULL, 
    CONSTRAINT [FK_Houses_Users] FOREIGN KEY ([Owner]) REFERENCES Users(Id) 
)
