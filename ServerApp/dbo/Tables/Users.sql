CREATE TABLE [dbo].[Users]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 

    [Username] VARCHAR(30) NULL, 

    [Password ] VARCHAR(32) NOT NULL, 

    [Email] VARCHAR(320) NOT NULL
)