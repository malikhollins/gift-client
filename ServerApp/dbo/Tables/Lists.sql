CREATE TABLE [dbo].[Lists]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,

    [House] INT NOT NULL,

    [Owner] INT NOT NULL,

    [ItemRankDescription] VARCHAR(250) NOT NULL, 

    -- The rank of all the items the user has formated to a string delimted ',' e.g. "listItemId 1,listItemId 2,...,listItemId X". 
    -- The amount of items a user can have in a list is limited to a constant.

    [Name] VARCHAR(30) NULL,

    CONSTRAINT [FK_Lists_ToUsers] FOREIGN KEY ([Owner]) REFERENCES [Users]([Id]),

    CONSTRAINT [FK_Lists_ToHouses] FOREIGN KEY ([House]) REFERENCES [Houses]([Id])
)

GO

CREATE INDEX [IX_Lists_House] ON [dbo].[Lists] ([House], [Owner])

