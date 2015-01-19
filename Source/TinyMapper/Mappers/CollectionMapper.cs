using System;
using System.Collections;

namespace TinyMapper.Mappers
{
    internal abstract class CollectionMapper
    {
        public object CopyTo(IEnumerable value)
        {
            return CopyToCore(value);
        }

        internal virtual object CopyToCore(IEnumerable value)
        {
            return null;
        }
    }
}
