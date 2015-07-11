using System;
using System.Linq.Expressions;

namespace Nelibur.ObjectMapper.Bindings
{
    internal sealed class BindingConfigOf<TSource, TTarget> : BindingConfig, IBindingConfig<TSource, TTarget>
    {
        public void Bind(Expression<Func<TSource, object>> source, Expression<Func<TTarget, object>> target)
        {
            string sourceName = GetMemberName(source);
            string targetName = GetMemberName(target);

            if (string.Equals(sourceName, targetName, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            BindFields(sourceName, targetName);
        }

        public void Ignore(Expression<Func<TTarget, object>> expression)
        {
            string memberName = GetMemberName(expression);
            IgnoreField(memberName);
        }

        private static string GetMemberName<T>(Expression<Func<T, object>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null)
            {
                var unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    member = unaryExpression.Operand as MemberExpression;
                }

                if (member == null)
                {
                    throw new ArgumentException("Expression is not a MemberExpression", "expression");
                }
            }
            return member.Member.Name;
        }
    }
}
