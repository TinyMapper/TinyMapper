using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators;
using TinyMapper.CodeGenerators.Ast;
using TinyMapper.Extensions;
using TinyMapper.Nelibur.Sword.Extensions;
using TinyMapper.TypeConverters;

namespace TinyMapper.Builders.Assemblies.Types.Members
{
    internal sealed class MemberBuilder
    {
        private readonly IMemberBuilderConfig _config;

        private MemberBuilder(IMemberBuilderConfig config)
        {
            _config = config;
        }

        public static IMemberBuilderConfig Configure(Action<IMemberBuilderConfig> action)
        {
            return new MemberBuilderConfig().Config(action);
        }

        public IAstNode Build(List<MappingMember> mappingMembers)
        {
            var result = new AstComposite();
            mappingMembers.ForEach(x => result.Add(Test(x)));
            return result;
        }

        private static IAstNode StoreFiled(FieldInfo field, IAstType targetObject, IAstType value)
        {
            return AstStoreField.Store(field, targetObject, value);
        }

        private static IAstNode StoreProperty(PropertyInfo property, IAstType targetObject, IAstType value)
        {
            return AstStoreProperty.Store(property, targetObject, value);
        }

        private static IAstNode StoreTargetObjectMember(MappingMember mappingMember, IAstType targetObject, IAstType convertedMember)
        {
            IAstNode result = null;
            mappingMember.Target
                         .ToOption()
                         .Match(x => x.IsField(), x => result = StoreFiled((FieldInfo)x, targetObject, convertedMember))
                         .Match(x => x.IsProperty(), x => result = StoreProperty((PropertyInfo)x, targetObject, convertedMember));
            return result;
        }

        private IAstType ConvertMember(MappingMember mappingMember, IAstType memberValue)
        {
            MethodInfo converter = GetTypeConverter(mappingMember);
            IAstType convertedMember = AstCallMethod.Call(converter, null, memberValue);
            return convertedMember;
        }

        private MethodInfo GetTypeConverter(MappingMember mappingMember)
        {
            MethodInfo result = PrimitiveTypeConverter.GetConverter(mappingMember.Source.GetMemberType(),
                mappingMember.Target.GetMemberType());
            return result;
        }

        private IAstType LoadField(IAstType source, FieldInfo field)
        {
            return AstLoadField.Load(source, field);
        }

        private IAstType LoadProperty(IAstType source, PropertyInfo property)
        {
            return AstLoadProperty.Load(source, property);
        }

        private IAstType LoadSourceObjectMember(MappingMember mappingMember, IAstType sourceObject)
        {
            IAstType result = null;
            mappingMember.Source
                         .ToOption()
                         .Match(x => x.IsField(), x => result = LoadField(sourceObject, (FieldInfo)x))
                         .Match(x => x.IsProperty(), x => result = LoadProperty(sourceObject, (PropertyInfo)x));
            return result;
        }

        private IAstNode Test(MappingMember mappingMember)
        {
            IAstType sourceObject = AstLoadLocal.Load(_config.LocalSource);

            IAstType memberValue = LoadSourceObjectMember(mappingMember, sourceObject);

            IAstType convertedMember = ConvertMember(mappingMember, memberValue);

            IAstType targetObject = AstLoadLocal.LoadAddress(_config.LocalTarget);

            IAstNode result = StoreTargetObjectMember(mappingMember, targetObject, convertedMember);
            return result;
        }


        private sealed class MemberBuilderConfig : IMemberBuilderConfig
        {
            public CodeGenerator CodeGenerator { get; set; }
            public LocalBuilder LocalSource { get; set; }
            public LocalBuilder LocalTarget { get; set; }

            public IMemberBuilderConfig Config(Action<IMemberBuilderConfig> action)
            {
                action(this);
                return this;
            }

            public MemberBuilder Create()
            {
                Validate();
                return new MemberBuilder(this);
            }

            private void Validate()
            {
                var nullCheck = new List<object>
                {
                    LocalSource, LocalTarget, CodeGenerator
                };

                if (nullCheck.Any(x => x.IsNull()))
                {
                    throw new ConfigurationErrorsException();
                }
            }
        }
    }
}
