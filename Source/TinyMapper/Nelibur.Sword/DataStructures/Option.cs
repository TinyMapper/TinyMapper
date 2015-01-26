using System;
using TinyMappers.Nelibur.Sword.Core;

namespace TinyMappers.Nelibur.Sword.DataStructures
{
    /// <summary>
    ///     https://github.com/Nelibur/Nelibur.
    /// </summary>
    internal sealed class Option<T>
    {
        private static readonly Option<T> _empty = new Option<T>(default(T), false);
        private readonly bool _hasValue;

        public Option(T value, bool hasValue = true)
        {
            _hasValue = hasValue;
            Value = value;
        }

        public static Option<T> Empty
        {
            get { return _empty; }
        }

        public bool HasNoValue
        {
            get { return !_hasValue; }
        }

        public bool HasValue
        {
            get { return _hasValue; }
        }

        public T Value { get; private set; }

        public Option<T> Match(Func<T, bool> predicate, Action<T> action)
        {
            if (HasNoValue)
            {
                return Empty;
            }
            if (predicate(Value))
            {
                action(Value);
            }
            return this;
        }

        public Option<T> MatchType<TTarget>(Action<TTarget> action)
            where TTarget : T
        {
            if (HasNoValue)
            {
                return Empty;
            }
            if (Value.GetType() == typeof(TTarget))
            {
                action((TTarget)Value);
            }
            return this;
        }

        public Option<T> ThrowOnEmpty<TException>()
            where TException : Exception, new()
        {
            if (HasValue)
            {
                return this;
            }
            throw Error.Type<TException>();
        }

        public Option<T> ThrowOnEmpty<TException>(Func<TException> func)
            where TException : Exception
        {
            if (HasValue)
            {
                return this;
            }
            throw func();
        }
    }
}
