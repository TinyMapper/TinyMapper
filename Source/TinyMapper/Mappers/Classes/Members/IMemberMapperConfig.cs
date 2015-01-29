using TinyMappers.CodeGenerators;
using TinyMappers.Reflection;

namespace TinyMappers.Mappers.Classes.Members
{
    internal interface IMemberMapperConfig
    {
        IDynamicAssembly Assembly { get; set; }
        CodeGenerator CodeGenerator { get; set; }
        MemberMapper Create();
    }
}
