using Nelibur.Mapper.CodeGenerators;
using Nelibur.Mapper.Reflection;

namespace Nelibur.Mapper.Mappers.Classes.Members
{
    internal interface IMemberMapperConfig
    {
        IDynamicAssembly Assembly { get; set; }
        CodeGenerator CodeGenerator { get; set; }
        MemberMapper Create();
    }
}
