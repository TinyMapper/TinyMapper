using System;
using System.Collections.Generic;

namespace TinyMappers.Nelibur.Sword.Patterns
{
    /// <summary>
    ///     Visitor pattern.
    /// </summary>
    /// <remarks>
    ///     I don't like to put more than one class in a CS file, but considering I'm expecting
    ///     users to add this file to their projects, so the single file will be the best choise.
    /// </remarks>
    public static class Visitor
    {
        public static IFuncVisitor<TBase, TResult> For<TBase, TResult>()
            where TBase : class
        {
            return new FuncVisitor<TBase, TResult>();
        }

        public static IActionVisitor<TBase> For<TBase>()
            where TBase : class
        {
            return new ActionVisitor<TBase>();
        }


        private sealed class ActionVisitor<TBase> : IActionVisitor<TBase>
            where TBase : class
        {
            private readonly Dictionary<Type, Action<TBase>> _repository =
                new Dictionary<Type, Action<TBase>>();

            public IActionVisitor<TBase> Register<T>(Action<T> action)
                where T : TBase
            {
                _repository[typeof(T)] = x => action((T)x);
                return this;
            }

            public void Visit<T>(T value)
                where T : TBase
            {
                Action<TBase> action = _repository[value.GetType()];
                action(value);
            }
        }


        private sealed class FuncVisitor<TBase, TResult> : IFuncVisitor<TBase, TResult>
            where TBase : class
        {
            private readonly Dictionary<Type, Func<TBase, TResult>> _repository =
                new Dictionary<Type, Func<TBase, TResult>>();

            public IFuncVisitor<TBase, TResult> Register<T>(Func<T, TResult> action)
                where T : TBase
            {
                _repository[typeof(T)] = x => action((T)x);
                return this;
            }

            public TResult Visit<T>(T value)
                where T : TBase
            {
                Func<TBase, TResult> action = _repository[value.GetType()];
                return action(value);
            }
        }
    }


    public interface IFuncVisitor<in TBase, TResult>
        where TBase : class
    {
        /// <summary>
        ///     Register action on <see cref="T" />.
        /// </summary>
        /// <typeparam name="T">Concrete type.</typeparam>
        /// <param name="action">Action.</param>
        IFuncVisitor<TBase, TResult> Register<T>(Func<T, TResult> action)
            where T : TBase;

        /// <summary>
        ///     Visit concrete type.
        /// </summary>
        /// <param name="value">Type to visit.</param>
        /// <returns>Result value.</returns>
        TResult Visit<T>(T value)
            where T : TBase;
    }


    public interface IActionVisitor<in TBase>
        where TBase : class
    {
        /// <summary>
        ///     Register action on <see cref="T" />.
        /// </summary>
        /// <typeparam name="T">Concrete type.</typeparam>
        /// <param name="action">Action.</param>
        IActionVisitor<TBase> Register<T>(Action<T> action)
            where T : TBase;

        /// <summary>
        ///     Visit concrete type.
        /// </summary>
        /// <param name="value">Type to visit.</param>
        void Visit<T>(T value)
            where T : TBase;
    }
}
