using System;
using System.Configuration;

namespace Nelibur.Mapper.Core
{
    /// <summary>
    ///     https://github.com/Nelibur/Nelibur.
    /// </summary>
    internal static class Error
    {
        public static Exception ArgumentNull(string paramName)
        {
            return new ArgumentNullException(paramName);
        }

        public static Exception ConfigurationError(string message)
        {
            return new ConfigurationErrorsException(message);
        }

        public static Exception InvalidOperation(string message)
        {
            return new InvalidOperationException(message);
        }

        public static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        public static Exception NotSupported()
        {
            return new NotSupportedException();
        }

        public static Exception Type<TException>()
            where TException : Exception, new()
        {
            return CreateCtor<TException>()();
        }

        private static Func<T> CreateCtor<T>()
            where T : new()
        {
            return () => new T();
        }
    }
}
