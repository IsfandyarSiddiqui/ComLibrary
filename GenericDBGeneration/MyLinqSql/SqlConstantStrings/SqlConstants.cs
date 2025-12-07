using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace SQLConstants
{
    public class Brackets
    {
        public const char Open = '(';
        public const char OpenCurly = '{';
        public const char OpenSquare = '[';
        public const char Close = ')';
        public const char CloseSquare = ']';
        public const char CloseCurly = '}';
    }
}

namespace SQLConstants.Operators
{
    public static class Comparison
    {
        public const string GREATER_THAN = " > ";
        public const string LESS_THAN = " < ";
        public const string EQUAL = " = ";
        public const string NOT_EQUAL = " <> ";
        public const string GREATER_THAN_OR_EQUAL = " >= ";
        public const string LESS_THAN_OR_EQUAL = " <= ";
    }

    public static class Logical
    {
        public const string And = " AND ";
        public const string Or = " OR ";
        public const string Not = " NOT ";
        public const string Like = "  LIKE ";
    }
}



#pragma warning restore IDE0130 // Namespace does not match folder structure
