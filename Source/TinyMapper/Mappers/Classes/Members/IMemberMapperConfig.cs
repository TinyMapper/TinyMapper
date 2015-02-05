using System;
using Nelibur.ObjectMapper.Reflection;

namespace Nelibur.ObjectMapper.Mappers.Classes.Members
{
    internal interface IMemberMapperConfig
    {
        IDynamicAssembly Assembly { get; set; }
        MemberMapper Create();
    }
}
