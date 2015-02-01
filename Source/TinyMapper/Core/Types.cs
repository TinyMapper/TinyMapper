using System;
using System.Collections;
using System.Collections.Generic;

namespace TinyMappers.Core
{
    internal static class Types
    {
        public static readonly Type IEnumerable = typeof(IEnumerable);
        public static readonly Type IEnumerableOf = typeof(IEnumerable<>);
    }
}
