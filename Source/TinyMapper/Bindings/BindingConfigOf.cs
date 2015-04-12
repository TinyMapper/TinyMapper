using System;
using System.Linq.Expressions;

namespace Nelibur.ObjectMapper.Bindings
{
    internal sealed class BindingConfigOf<TTarget> : BindingConfig, IBindingConfig<TTarget>
    {
        public void Ignore(Expression<Func<TTarget, object>> expression)
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
            IgnoreField(member.Member.Name);
        }
    }
}
