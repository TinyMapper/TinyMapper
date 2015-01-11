using System;
using System.Collections.Generic;
using System.Reflection;
using TinyMapper.CodeGenerators.Ast;
using TinyMapper.Nelibur.Sword.DataStructures;
using TinyMapper.Nelibur.Sword.Extensions;

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

        private MethodInfo GetTypeConverter()
        {
            throw new NotImplementedException();
        }

        private IAstType ReadField(IAstType source, FieldInfo field)
        {
            return AstLoadField.Load(source, field);
        }

        private IAstType ReadField(IAstType source, PropertyInfo property)
        {
            throw new NotImplementedException();
        }

        private Option<IAstType> Test(MappingMember mappingMember)
        {
            IAstType sourceObject = AstLoadLocal.Load(_config.LocalSource);

            IAstType sourceValue = null;

            mappingMember.Source
                         .ToOption()
                         .MatchType<FieldInfo>(x => sourceValue = ReadField(sourceObject, x))
                         .MatchType<PropertyInfo>(x => sourceValue = ReadField(sourceObject, x));

            return sourceValue.ToOption();
        }
    }
}
