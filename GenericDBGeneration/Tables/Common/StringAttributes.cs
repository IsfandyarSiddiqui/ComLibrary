using LinqToDB;
using LinqToDB.Mapping;

namespace GenericDBGeneration.Tables.Common;
public class StrAttribute : ColumnAttribute
{
    public StrAttribute(int length)
    {
        DataType = DataType.NVarChar;
        Length = length;
    }
}

public class Str32Attribute : StrAttribute { public Str32Attribute() : base(32) { } }
public class Str64Attribute : StrAttribute { public Str64Attribute() : base(64) { } }

public class Str255Attribute : StrAttribute { public Str255Attribute() : base(255) { } }