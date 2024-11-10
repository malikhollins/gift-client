CREATE TABLE [dbo].[Households]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [OwnerId] INT NOT NULL, 
    [Name] VARCHAR(30) NULL, 
    [GiftingType] ENUM('hello'),
    CONSTRAINT [FK_Households_ToUsers] FOREIGN KEY ([OwnerId]) REFERENCES [Users](Id)
)
