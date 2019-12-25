using System;
using System.Linq.Expressions;

namespace Vaccination.Infastructure.Utils
{
    public static class ExpressionHelper
	{
        public static MemberExpression GetMemberExpression<TMember>(this Expression<Func<TMember>> expression)
        {
            var lambda = (LambdaExpression)expression;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            return memberExpression;
        }
    }
}
