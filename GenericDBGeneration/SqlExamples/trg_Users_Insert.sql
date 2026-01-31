CREATE OR ALTER TRIGGER [dbo].[trg_Users_Insert]
ON [dbo].[Users]
AFTER INSERT
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
        1, -- 1 = Insert
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