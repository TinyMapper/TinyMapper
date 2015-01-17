using System;
using System.Reflection.Emit;
using TinyMapper.Mappers.Builders.Types;

namespace TinyMapper.Reflection
{
    internal interface IDynamicAssembly
    {
        TypeBuilder DefineType(string typeName, Type parentType);
        TargetMapperBuilder GetTypeBuilder();
        void Save();
    }
}
