using System;
using Nelibur.Mapper.Reflection;

namespace Nelibur.Mapper.Mappers.Classes.Members
{
    internal interface IMemberMapperConfig
    {
        IDynamicAssembly Assembly { get; set; }
        MemberMapper Create();
    }
}
