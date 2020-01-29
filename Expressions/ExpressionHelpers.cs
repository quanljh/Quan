using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Quan.Expressions
{
    /// <summary>
    /// A help for expressions
    /// </summary>
    public static class ExpressionHelpers
    {
        /// <summary>
        /// Compile an expression and gets the functions return value
        /// </summary>
        /// <typeparam name="T">The type of return value</typeparam>
        /// <param name="lambda">The expression to compile</param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this Expression<Func<T>> lambda)
        {
            return lambda.Compile().Invoke();
        }

        /// <summary>
        /// Sets the underlying properties value to the given value
        /// from an expression that contains the property
        /// </summary>
        /// <typeparam name="T">The type of the value to set</typeparam>
        /// <param name="lambda">The expression</param>
        /// <param name="value">The value to set the property to</param>
        public static void SetPropertyValue<T>(this Expression<Func<T>> lambda, T value)
        {
            //Converts a lambda () => some.Property, to some.Property
            if (lambda is LambdaExpression lambdaExpression && lambdaExpression.Body is MemberExpression expression)
            {
                //Get the property information so we can set it 
                if (expression.Member is PropertyInfo propertyInfo)
                {
                    var target = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();

                    //Set the property value
                    propertyInfo.SetValue(target, value);
                }
            }

        }
    }
}
