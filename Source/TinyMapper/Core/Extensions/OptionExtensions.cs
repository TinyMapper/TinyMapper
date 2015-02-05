using System;
using Nelibur.ObjectMapper.Core.DataStructures;

namespace Nelibur.ObjectMapper.Core.Extensions
{
    /// <summary>
    ///     https://github.com/Nelibur/Nelibur.
    /// </summary>
    internal static class OptionExtensions
    {
        public static Option<T> Do<T>(this Option<T> value, Action<T> action)
        {
            if (value.HasValue)
            {
                action(value.Value);
            }
            return value;
        }

        public static Option<T> DoOnEmpty<T>(this Option<T> value, Action action)
        {
            if (value.HasNoValue)
            {
                action();
            }
            return value;
        }

        public static Option<T> Finally<T>(this Option<T> value, Action<T> action)
        {
            action(value.Value);
            return value;
        }

        public static Option<TResult> Map<TInput, TResult>(this Option<TInput> value, Func<TInput, Option<TResult>> func)
        {
            if (value.HasNoValue)
            {
                return Option<TResult>.Empty;
            }
            return func(value.Value);
        }

        public static Option<TResult> Map<TInput, TResult>(this Option<TInput> value, Func<TInput, TResult> func)
        {
            if (value.HasNoValue)
            {
                return Option<TResult>.Empty;
            }
            return func(value.Value).ToOption();
        }

        public static Option<TResult> Map<TInput, TResult>(this Option<TInput> value, Func<TInput, bool> predicate, Func<TInput, TResult> func)
        {
            if (value.HasNoValue)
            {
                return Option<TResult>.Empty;
            }
            if (!predicate(value.Value))
            {
                return Option<TResult>.Empty;
            }
            return func(value.Value).ToOption();
        }

        public static Option<T> MapOnEmpty<T>(this Option<T> value, Func<T> func)
        {
            if (value.HasNoValue)
            {
                return func().ToOption();
            }
            return value;
        }

        public static Option<V> SelectMany<T, U, V>(this Option<T> value, Func<T, Option<U>> func, Func<T, U, V> selector)
        {
            return value.Map(x => func(x).Map(y => selector(x, y).ToOption()));
        }

        public static Option<T> Where<T>(this Option<T> value, Func<T, bool> predicate)
        {
            if (value.HasNoValue)
            {
                return Option<T>.Empty;
            }
            return predicate(value.Value) ? value : Option<T>.Empty;
        }
    }
}
