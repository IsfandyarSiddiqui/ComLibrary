using LinqToDB;
using LinqToDB.Mapping;

namespace GenericDBGeneration.Tables.Common;
public class StrAttribute : ColumnAttribute
{
    public StrAttribute(int length, bool canBeNull)
    {
        DataType = DataType.NVarChar;
        Length = length;
        CanBeNull = canBeNull;
    }
}

public class Str32Attribute(bool nullable = false) : StrAttribute(32, nullable) { }

public class Str64Attribute(bool nullable = false) : StrAttribute(64, nullable) { }

public class Str255Attribute(bool nullable = false) : StrAttribute(255, nullable) { }