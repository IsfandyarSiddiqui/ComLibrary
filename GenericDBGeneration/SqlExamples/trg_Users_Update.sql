CREATE OR ALTER TRIGGER [dbo].[trg_Users_Update]
ON [dbo].[Users]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[UsersLogs] (
        [originalId], 
        [actionType], 
        [lastActionDate], 
        [lastActionByUserId], 
        [changes]
    )
    SELECT 
        i.[userId], 
        2, -- 2 = Update
        i.lastActionDate, 
        i.lastActionByUserId, 
        (
            SELECT * FROM inserted sub 
            WHERE sub.[userId] = i.[userId] 
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        )
    FROM inserted i;
END;
GO