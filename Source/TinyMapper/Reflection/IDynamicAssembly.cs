using System;
using System.Reflection.Emit;
using TinyMapper.Mappers;

namespace TinyMapper.Reflection
{
    internal interface IDynamicAssembly
    {
        TypeBuilder DefineType(string typeName, Type parentType);
        TargetMapperBuilder GetTypeBuilder();
        void Save();
    }
}
