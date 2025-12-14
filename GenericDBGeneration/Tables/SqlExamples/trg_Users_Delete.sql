CREATE OR ALTER TRIGGER [dbo].[trg_Users_Delete]
ON [dbo].[Users]
AFTER DELETE
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
        d.[userId], 
        3, -- 3 = Delete
        i.lastActionDate, 
        i.lastActionByUserId,
        (
            SELECT * FROM deleted sub 
            WHERE sub.[userId] = d.[userId] 
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        )
    FROM deleted d;
END;
GO