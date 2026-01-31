namespace GenericDBGeneration.Triggers;

public static class LogginTriggers
{
    public static string Create(string baseTable, string logTable, string primaryKey, TriggerType tt) =>
        $"""
        GO
        CREATE OR ALTER TRIGGER [dbo].[trg_{baseTable}_{tt}]
        ON [dbo].[{baseTable}]
        AFTER {tt}
        AS
        BEGIN
            SET NOCOUNT ON;

            INSERT INTO [dbo].[{logTable}] (
                [originalId], 
                [actionType], 
                [lastActionDate], 
                [lastActionByUserId], 
                [changes]
            )Tr
            SELECT 
                iud.[{primaryKey}], 
                {(int)tt},
                iud.lastActionDate, 
                iud.lastActionByUserId,
                (
                    SELECT * FROM {ToTriggerTable(tt)} sub 
                    WHERE sub.[{primaryKey}] = iud.[{primaryKey}] 
                    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                )
            FROM inserted iud;
        END;
        GO
        """;

    public static string ToTriggerTable(TriggerType tt) => tt switch
    {
        TriggerType.Insert => "Inserted",
        TriggerType.Update => "Inserted",
        TriggerType.Delete => "Deleted",
        _ => throw new NotImplementedException(),
    };
}

public enum TriggerType { Insert, Update, Delete }

