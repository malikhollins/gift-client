CREATE TABLE [dbo].[UserToHouse]
(
	[User] INT NOT NULL PRIMARY KEY, 
    [House] INT NOT NULL, 
    CONSTRAINT [FK_UserToHouse_Users] FOREIGN KEY ([User]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_UserToHouse_House] FOREIGN KEY ([House]) REFERENCES [Houses]([Id])
)

GO

CREATE INDEX [IX_UserToHouse_UserHouse] ON [dbo].[UserToHouse] ([User], [House])


