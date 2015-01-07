using System;
using System.Reflection.Emit;
using TinyMapper.Mappers;

namespace TinyMapper.Engines
{
    internal sealed class MappingEngine
    {
        public void CreateMapper(Type sourceType, Type targetType)
        {
            TypeBuilder typeBuilder = DynamicAssembly.DefineType("Test1", typeof(MarkerTypeMapper));
        }
    }
}
