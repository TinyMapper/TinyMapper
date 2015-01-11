using System;
using System.Collections.Generic;
using TinyMapper.CodeGenerators.Ast;

namespace TinyMapper.Builders.Assemblies.Types.Members
{
    internal sealed class MemberBuilder
    {
        private readonly IMemberBuilderConfig _config;

        public MemberBuilder(IMemberBuilderConfig config)
        {
            _config = config;
        }

        public IAstNode Build(List<MappingMember> mappingMembers)
        {
            throw new NotImplementedException();
        }
    }
}
