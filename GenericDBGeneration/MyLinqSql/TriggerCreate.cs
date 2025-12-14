using GenericDBGeneration.Tables.Common;
using System.Linq.Expressions;
using System.Text;

namespace GenericDBGeneration.MyLinqSql;

public class TriggerHelper<T1,T2> where T1: LogableBase where T2: LogableBase
{
    private const string LogTemplate =
    @"
    CREATE OR ALTER TRIGGER [dbo].[trg_{0}_Insert]
    ON [dbo].[{0}]
    AFTER INSERT
    AS
    BEGIN
        SET NOCOUNT ON;
        INSERT INTO [dbo].[{1}] (
            {4}
        )
        SELECT 
            i.[5], 
            1, -- 1 = Insert
            i.lastActionDate, 
            i.{lastActionByUserId},
            (
                SELECT * FROM inserted sub 
                WHERE sub.[5] = i.[5] 
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            )
        FROM inserted i;
    END;
    GO
    ";

    // LogableBase Columns
    public static IReadOnlyList<string> logableBaseColumns =
        [nameof(LogableBase.lastActionByUserId), nameof(LogableBase.lastActionDate)];
    
    // LogTableBase Columns
    public static IReadOnlyList<string> logTableBaseColumns =
    [
        nameof(LogableBase.lastActionByUserId), nameof(LogableBase.lastActionDate),
        nameof(LogTableBase.logId), nameof(LogTableBase.changes),
        nameof(LogTableBase.actionType), nameof(LogTableBase.originalId)
    ];

    public string Create(Expression expression)
    {
        Type tableType = typeof(T1);
        Type logTableType = typeof(T2);
        var tableName = tableType.Name;
        var logTableName = logTableType.Name;

        List<string> tableColumns = [];
        foreach (var prop in logTableType.GetProperties())
        {
            if(logTableBaseColumns.Contains(prop.Name)) continue;
            tableColumns.Add(prop.Name);
        }

        string s =
        """
            u => new { u.userId, u.fullName, u.email }
        """;

        return string.Format(LogTemplate, tableName, logTableName);
    }
}

public enum TriggerType { Insert, Update, Delete }

