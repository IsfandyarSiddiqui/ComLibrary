using LinqToDB.Mapping;

namespace GenericDBGeneration.Tables.Common;

public abstract class LogableBase
{
    [Column(DbType = "DateTime2 DEFAULT GETUTCDATE()", CanBeNull = false, SkipOnInsert = true, SkipOnUpdate = true)] 
    public DateTime lastActionDate { get; set; }
    
    [NotNull] 
    public int lastActionByUserId { get; set; }
}

public abstract class LogTableBase: LogableBase
{
    protected LogTableBase(LogableBase logableBase, SQLActionType sqlActionType)
    {
        lastActionByUserId = logableBase.lastActionByUserId;
        lastActionDate = logableBase.lastActionDate;
        logId = -1;
        changes = string.Empty;
        actionType = sqlActionType;
        originalId = -1;
    }

    [PrimaryKey, Identity] public int logId { get; set; } // Log tables need their own PK
    [Column(DbType = "NVARCHAR(MAX)"), NotNull] public required string changes { get; set; }
    public SQLActionType actionType { get; set; }
    public abstract int originalId { get; set; } 
}

public enum SQLActionType
{
    Insert = 1,
    Update = 2,
    Delete = 3
}
