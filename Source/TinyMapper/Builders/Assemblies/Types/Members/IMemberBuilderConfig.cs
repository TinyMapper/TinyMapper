using System.Reflection.Emit;
using TinyMapper.CodeGenerators;

namespace TinyMapper.Builders.Assemblies.Types.Members
{
    internal interface IMemberBuilderConfig
    {
        CodeGenerator CodeGenerator { get; set; }
        LocalBuilder LocalSource { get; set; }
        LocalBuilder LocalTarget { get; set; }
        MemberBuilder Create();
    }
}
