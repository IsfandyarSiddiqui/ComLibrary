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
    [PrimaryKey, Identity] public int logId { get; set; } // Log tables need their own PK

    public int originalId { get; set; } 

    public SQLActionType actionType { get; set; }

    [Column(DbType = "NVARCHAR(MAX)"), NotNull] public required string changes { get; set; }
}

public enum SQLActionType
{
    Insert = 1,
    Update = 2,
    Delete = 3
}
