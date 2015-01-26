using System;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers.Classes.Members
{
    internal interface IMemberMapperConfig
    {
        IDynamicAssembly Assembly { get; set; }
        CodeGenerator CodeGenerator { get; set; }
        LocalBuilder LocalSource { get; set; }
        LocalBuilder LocalTarget { get; set; }
        MemberMapper Create();
    }
}
