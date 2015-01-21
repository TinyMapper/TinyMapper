using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators;
using TinyMapper.CodeGenerators.Emitters;
using TinyMapper.Extensions;
using TinyMapper.Mappers.Types.Members;
using TinyMapper.Nelibur.Sword.Extensions;
using TinyMapper.TypeConverters;

namespace TinyMapper.Mappers.Builders.Members
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

        public IEmitter Build(List<MappingMember> mappingMembers)
        {
            var result = new EmitterComposite();
            mappingMembers.ForEach(x => result.Add(Build(x)));
            return result;
        }

        private static IEmitterType StoreFiled(FieldInfo field, IEmitterType targetObject, IEmitterType value)
        {
            return EmitterField.Store(field, targetObject, value);
        }

        private static IEmitterType StoreProperty(PropertyInfo property, IEmitterType targetObject, IEmitterType value)
        {
            return EmitterProperty.Store(property, targetObject, value);
        }

        private static IEmitterType StoreTargetObjectMember(MappingMember mappingMember, IEmitterType targetObject, IEmitterType convertedMember)
        {
            IEmitterType result = null;
            mappingMember.Target
                         .ToOption()
                         .Match(x => x.IsField(), x => result = StoreFiled((FieldInfo)x, targetObject, convertedMember))
                         .Match(x => x.IsProperty(), x => result = StoreProperty((PropertyInfo)x, targetObject, convertedMember));
            return result;
        }

        private IEmitter Build(MappingMember mappingMember)
        {
            IEmitterType sourceObject = EmitterLocal.Load(_config.LocalSource);

            IEmitterType memberValue = LoadSourceObjectMember(mappingMember, sourceObject);

            IEmitterType convertedMember = ConvertMember(mappingMember, memberValue);

            IEmitterType targetObject = EmitterLocal.LoadAddress(_config.LocalTarget);

            IEmitter result = StoreTargetObjectMember(mappingMember, targetObject, convertedMember);
            return result;
        }

        private IEmitterType ConvertMember(MappingMember mappingMember, IEmitterType memberValue)
        {
            MethodInfo converter = GetTypeConverter(mappingMember);
            IEmitterType convertedMember = EmitterMethod.Call(converter, null, memberValue);
            return convertedMember;
        }

        private MethodInfo GetTypeConverter(MappingMember mappingMember)
        {
            if (mappingMember is PrimitiveMappingMember)
            {
                return PrimitiveTypeConverter.GetConverter(mappingMember.TypePair);
            }
            else if (mappingMember is ComplexMappingMember)
            {
                if (CollectionTypeConverter.IsSupported(mappingMember.TypePair))
                {
                    return CollectionTypeConverter.GetConverter(mappingMember.TypePair);
                }
            }
            throw new NotSupportedException();
        }

        private IEmitterType LoadField(IEmitterType source, FieldInfo field)
        {
            return EmitterField.Load(source, field);
        }

        private IEmitterType LoadProperty(IEmitterType source, PropertyInfo property)
        {
            return EmitterProperty.Load(source, property);
        }

        private IEmitterType LoadSourceObjectMember(MappingMember mappingMember, IEmitterType sourceObject)
        {
            IEmitterType result = null;
            mappingMember.Source
                         .ToOption()
                         .Match(x => x.IsField(), x => result = LoadField(sourceObject, (FieldInfo)x))
                         .Match(x => x.IsProperty(), x => result = LoadProperty(sourceObject, (PropertyInfo)x));
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
