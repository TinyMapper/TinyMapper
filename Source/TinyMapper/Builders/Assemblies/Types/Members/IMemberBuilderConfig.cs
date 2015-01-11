using System;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators;

namespace TinyMapper.Builders.Assemblies.Types.Members
{
    internal interface IMemberBuilderConfig
    {
        LocalBuilder LocalSource { get; set; }
        LocalBuilder LocalTarget { get; set; }
        CodeGenerator CodeGenerator { get; set; }
        IMemberBuilderConfig Configure(Action<IMemberBuilderConfig> action);
        MemberBuilder CreateMemberBuilder();
    }
}
