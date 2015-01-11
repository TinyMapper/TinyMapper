using System;
using System.Collections.Generic;
using System.Reflection;
using TinyMapper.CodeGenerators.Ast;
using TinyMapper.Extensions;
using TinyMapper.Nelibur.Sword.Extensions;
using TinyMapper.TypeConverters;

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

        private void Test(MappingMember mappingMember)
        {
            IAstType sourceObject = AstLoadLocal.Load(_config.LocalSource);

            IAstType memberValue = null;

            mappingMember.Source
                         .ToOption()
                         .MatchType<FieldInfo>(x => memberValue = ReadField(sourceObject, x))
                         .MatchType<PropertyInfo>(x => memberValue = ReadField(sourceObject, x));

            MethodInfo converter = PrimitiveTypeConverter.GetConverter(mappingMember.Source.GetMemberType(),
                mappingMember.Target.GetMemberType());

            IAstType convertedMember = AstCallMethod.Call(converter, null, memberValue);
        }
    }
}
