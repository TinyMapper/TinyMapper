using System;
using System.Reflection;

namespace Nelibur.Mapper.Core.Extensions
{
    internal static class MethodInfoExtensions
    {
        public static Func<T, TResult> ToFunc<T, TResult>(this MethodInfo value)
        {
            if (value.GetParameters().Length != 1)
            {
                throw new ArgumentException();
            }
            var result = (Func<T, TResult>)Delegate.CreateDelegate(typeof(Func<T, TResult>), value);
            return result;
        }
    }
}
