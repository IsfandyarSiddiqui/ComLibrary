using LinqToDB.Mapping;
using System.Runtime.CompilerServices;

namespace GenericDBGeneration.Tables.Common;

public abstract class LogableBase: ITableTag
{
    [Column(DbType = "DateTime2 DEFAULT GETUTCDATE()", CanBeNull = false, SkipOnInsert = true, SkipOnUpdate = true)] 
    public DateTime lastActionDate { get; set; }
    
    [NotNull] 
    public int lastActionByUserId { get; set; }
}

public class ConcreteLogableBase : LogableBase { }
public class ConcreteLogTableBase : LogTableBase
{
    public ConcreteLogTableBase(ConcreteLogableBase logableBase, SQLActionType sqlActionType) 
        : base(logableBase, sqlActionType) { }
    public override int originalId { get; set; } = -1;

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
