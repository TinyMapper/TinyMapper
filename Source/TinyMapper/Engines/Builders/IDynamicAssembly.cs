using System;
using System.Reflection.Emit;

namespace TinyMapper.Engines.Builders
{
    internal interface IDynamicAssembly
    {
        TypeBuilder DefineType(string typeName, Type parentType);
        void Save();
    }
}