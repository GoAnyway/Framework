using System.Linq.Expressions;

namespace GoAnyway.Framework.Extensions;

public static class ExpressionExtensions
{
    public static string GetMemberName<T>(this Expression<T> expression)
    {
        return expression.Body switch
        {
            MemberExpression m => m.Member.Name,
            UnaryExpression { Operand: MemberExpression m } => m.Member.Name,
            _ => throw new ArgumentOutOfRangeException(nameof(expression), expression, null)
        };
    }
}