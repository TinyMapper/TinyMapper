using System;
using System.Reflection.Emit;
using TinyMappers.Mappers;

namespace TinyMappers.Reflection
{
    internal interface IDynamicAssembly
    {
        TypeBuilder DefineType(string typeName, Type parentType);
        TargetMapperBuilder GetTypeBuilder();
        void Save();
    }
}
