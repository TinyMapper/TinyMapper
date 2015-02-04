using System;
using System.Reflection.Emit;
using Nelibur.Mapper.Mappers;

namespace Nelibur.Mapper.Reflection
{
    internal interface IDynamicAssembly
    {
        TypeBuilder DefineType(string typeName, Type parentType);
        TargetMapperBuilder GetTypeBuilder();
        void Save();
    }
}
