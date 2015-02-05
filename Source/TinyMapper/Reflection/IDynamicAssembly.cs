using System;
using System.Reflection.Emit;
using Nelibur.ObjectMapper.Mappers;

namespace Nelibur.ObjectMapper.Reflection
{
    internal interface IDynamicAssembly
    {
        TypeBuilder DefineType(string typeName, Type parentType);
        TargetMapperBuilder GetTypeBuilder();
        void Save();
    }
}
