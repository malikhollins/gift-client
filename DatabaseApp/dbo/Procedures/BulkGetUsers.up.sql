CREATE OR ALTER PROCEDURE [dbo].[BulkGetUsers]
	@email varchar(64)
AS
    DECLARE @searchPattern varchar(68);
BEGIN
    SET @searchPattern = '"' + @email + '*"';
	
	SELECT [dbo].[Users].[Id], [dbo].[Users].[Name], [dbo].[Users].[Email]
	FROM [dbo].[Users]
	WHERE CONTAINS([dbo].[Users].[Email], @searchPattern);
END;
