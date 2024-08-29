using System.Linq.Expressions;
using System.Reflection;

namespace NArtadoSearch.Core.Utilities.Mapping.Helpers;

public class ExpressionHelper
{
    public static PropertyInfo? GetProperty<T>(Expression<Func<T, object>> expression)
    {
        if (expression.Body is UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MemberExpression memberExpression)
            {
                return (PropertyInfo)memberExpression.Member;
            }
        }
        else if (expression.Body is MemberExpression member)
        {
            return (PropertyInfo)member.Member;
        }

        return null;
    }
}