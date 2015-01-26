using System.Reflection.Emit;
using TinyMappers.CodeGenerators;
using TinyMappers.Reflection;

namespace TinyMappers.Mappers.Classes.Members
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
