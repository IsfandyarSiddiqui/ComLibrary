using System.Text;
using System.Linq.Expressions;
using SQLConstants;
using SQLConstants.Operators;
using GenericDBGeneration.Tables;

namespace GenericDBGeneration.MyLinqSql;

public class SimpleSqlVisitor : ExpressionVisitor
{
    private StringBuilder sb = new();

    public string Translate(Expression expression)
    {
        sb.Clear();
        Visit(expression); // Starts the recursion
        return sb.ToString();
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        sb.Append(Brackets.Open);
        Visit(node.Left); // Visit Left side

        string temp = BinaryExpressionToString(node);
        sb.Append(temp);

        Visit(node.Right); // Visit Right side
        sb.Append(Brackets.Close);
        return node;
    }

    private static string BinaryExpressionToString(BinaryExpression node) =>
    node.NodeType switch
    {
        // Comparison Operations
        ExpressionType.GreaterThan => Comparison.GREATER_THAN,
        ExpressionType.GreaterThanOrEqual => Comparison.GREATER_THAN_OR_EQUAL,
        ExpressionType.LessThan => Comparison.LESS_THAN,
        ExpressionType.LessThanOrEqual => Comparison.LESS_THAN_OR_EQUAL,
        ExpressionType.Equal => Comparison.EQUAL,
        ExpressionType.NotEqual => Comparison.NOT_EQUAL,

        // Logical Operations (Boolean)
        ExpressionType.AndAlso => Logical.And,
        ExpressionType.OrElse => Logical.Or,

        // Arithmetic Operations
        ExpressionType.Add => Arithmetic.Add,
        ExpressionType.Subtract => Arithmetic.Subtract,
        ExpressionType.Multiply => Arithmetic.Multiply,
        ExpressionType.Divide => Arithmetic.Divide,
        ExpressionType.Modulo => Arithmetic.Modulo,

        // Bitwise Operations (Integer)
        ExpressionType.And => Bitwise.And,
        ExpressionType.Or => Bitwise.Or,
        ExpressionType.ExclusiveOr => throw new NotImplementedException(), //Bitwise.Xor,

        // Assignment Operations
        ExpressionType.Assign => Assignment.Equal,
        ExpressionType.AddAssign => throw new NotImplementedException(),
        ExpressionType.SubtractAssign => throw new NotImplementedException(),
        ExpressionType.MultiplyAssign => throw new NotImplementedException(),
        ExpressionType.DivideAssign => throw new NotImplementedException(),
        ExpressionType.ModuloAssign => throw new NotImplementedException(),
        ExpressionType.AndAssign => throw new NotImplementedException(),
        ExpressionType.OrAssign => throw new NotImplementedException(),
        ExpressionType.ExclusiveOrAssign => throw new NotImplementedException(),

        // Bit shifting (T-SQL does not have standard << >> operators)
        ExpressionType.LeftShift => throw new NotImplementedException("<< not supported in standard SQL."),
        ExpressionType.RightShift => throw new NotImplementedException(">> not supported in standard SQL."),
        ExpressionType.LeftShiftAssign => throw new NotImplementedException("<<= not supported in standard SQL."),
        ExpressionType.RightShiftAssign => throw new NotImplementedException(">>= not supported in standard SQL."),

        // Checked Arithmetic (Maps to standard operators)
        ExpressionType.AddChecked => Arithmetic.Add,
        ExpressionType.SubtractChecked => Arithmetic.Subtract,
        ExpressionType.MultiplyChecked => Arithmetic.Multiply,
        ExpressionType.AddAssignChecked => throw new NotImplementedException(),
        ExpressionType.SubtractAssignChecked => throw new NotImplementedException(),
        ExpressionType.MultiplyAssignChecked => throw new NotImplementedException(),

        // Unary/Other nodes technically not valid in "BinaryExpression" context but listed for completeness
        ExpressionType.Power => throw new NotImplementedException("^ is not a standard binary operator in SQL (Use POWER() function)."),
        ExpressionType.Coalesce => throw new NotImplementedException("?? requires function mapping (COALESCE())."),
        ExpressionType.ArrayIndex => throw new NotImplementedException("ArrayIndex not supported directly."),
        ExpressionType.ArrayLength => throw new NotImplementedException(),
        ExpressionType.Call => throw new NotImplementedException(),
        ExpressionType.Conditional => throw new NotImplementedException(),
        ExpressionType.Constant => throw new NotImplementedException(),
        ExpressionType.Convert => throw new NotImplementedException(),
        ExpressionType.ConvertChecked => throw new NotImplementedException(),
        ExpressionType.Invoke => throw new NotImplementedException(),
        ExpressionType.Lambda => throw new NotImplementedException(),
        ExpressionType.ListInit => throw new NotImplementedException(),
        ExpressionType.MemberAccess => throw new NotImplementedException(),
        ExpressionType.MemberInit => throw new NotImplementedException(),
        ExpressionType.Negate => throw new NotImplementedException(),
        ExpressionType.UnaryPlus => throw new NotImplementedException(),
        ExpressionType.NegateChecked => throw new NotImplementedException(),
        ExpressionType.New => throw new NotImplementedException(),
        ExpressionType.NewArrayInit => throw new NotImplementedException(),
        ExpressionType.NewArrayBounds => throw new NotImplementedException(),
        ExpressionType.Not => throw new NotImplementedException(),
        ExpressionType.Parameter => throw new NotImplementedException(),
        ExpressionType.Quote => throw new NotImplementedException(),
        ExpressionType.TypeAs => throw new NotImplementedException(),
        ExpressionType.TypeIs => throw new NotImplementedException(),
        ExpressionType.Block => throw new NotImplementedException(),
        ExpressionType.DebugInfo => throw new NotImplementedException(),
        ExpressionType.Decrement => throw new NotImplementedException(),
        ExpressionType.Dynamic => throw new NotImplementedException(),
        ExpressionType.Default => throw new NotImplementedException(),
        ExpressionType.Extension => throw new NotImplementedException(),
        ExpressionType.Goto => throw new NotImplementedException(),
        ExpressionType.Increment => throw new NotImplementedException(),
        ExpressionType.Index => throw new NotImplementedException(),
        ExpressionType.Label => throw new NotImplementedException(),
        ExpressionType.RuntimeVariables => throw new NotImplementedException(),
        ExpressionType.Loop => throw new NotImplementedException(),
        ExpressionType.Switch => throw new NotImplementedException(),
        ExpressionType.Throw => throw new NotImplementedException(),
        ExpressionType.Try => throw new NotImplementedException(),
        ExpressionType.Unbox => throw new NotImplementedException(),
        ExpressionType.PowerAssign => throw new NotImplementedException(),
        ExpressionType.PreIncrementAssign => throw new NotImplementedException(),
        ExpressionType.PreDecrementAssign => throw new NotImplementedException(),
        ExpressionType.PostIncrementAssign => throw new NotImplementedException(),
        ExpressionType.PostDecrementAssign => throw new NotImplementedException(),
        ExpressionType.TypeEqual => throw new NotImplementedException(),
        ExpressionType.OnesComplement => throw new NotImplementedException(),
        ExpressionType.IsTrue => throw new NotImplementedException(),
        ExpressionType.IsFalse => throw new NotImplementedException(),

        // Default
        //_ => throw new NotImplementedException($"Binary expression type '{node.NodeType}' is not supported.")
    };

    protected override Expression VisitMember(MemberExpression node)
    {
        sb.Append(Brackets.OpenSquare);
        sb.Append(node.Member.Name);
        sb.Append(Brackets.CloseSquare);
        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        // Handles values like 18 or "Admin"
        if(node.Value is string)
            sb.Append($"'{node.Value}'");
        else
            sb.Append(node.Value);
        return node;
    }
}