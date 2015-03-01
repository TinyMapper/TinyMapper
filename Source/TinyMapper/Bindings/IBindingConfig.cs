using System;
using System.Linq.Expressions;

namespace Nelibur.ObjectMapper.Bindings
{
    public interface IBindingConfig<TTarget>
    {
        void Ignore(Expression<Func<TTarget, object>> expression);
    }
}
