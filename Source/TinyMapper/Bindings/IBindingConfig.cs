using System;
using System.Linq.Expressions;

namespace Nelibur.ObjectMapper.Bindings
{
    public interface IBindingConfig<TSource, TTarget>
    {
        void Bind(Expression<Func<TSource, object>> source, Expression<Func<TTarget, object>> target);

        //        void Bind<TField>(Expression<Func<TTarget, TField>> target, TField value); not working yet
        void Bind(Expression<Func<TTarget, object>> target, Type targetType);

        void Ignore(Expression<Func<TSource, object>> expression);
    }
}
