using System;
using System.Reflection.Emit;
using TinyMapper.Builders.Assemblies.Types;

namespace TinyMapper.Builders.Assemblies
{
    internal interface IDynamicAssembly
    {
        TypeBuilder DefineType(string typeName, Type parentType);
        TargetMapperBuilder GetTypeBuilder();
        void Save();
    }
}
