using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators;
using TinyMapper.CodeGenerators.Emitters;
using TinyMapper.Extensions;
using TinyMapper.Mappers.Collections;
using TinyMapper.Mappers.Types1.Members;
using TinyMapper.Nelibur.Sword.Extensions;
using TinyMapper.Reflection;
using TinyMapper.TypeConverters;

namespace TinyMapper.Mappers.Classes.Members
{
    internal sealed class MemberMapper
    {
        private readonly IMemberMapperConfig _config;

        private readonly MapperCache _mappers = new MapperCache();

        private MemberMapper(IMemberMapperConfig config)
        {
            _config = config;
        }

        public static IMemberMapperConfig Configure(Action<IMemberMapperConfig> action)
        {
            return new MemberMapperConfig().Config(action);
        }

        public MemberEmitterDescription Build(List<MappingMember> members)
        {
            var emitter = new EmitterComposite();
            members.ForEach(x => emitter.Add(Build(x)));
            var result = new MemberEmitterDescription(emitter, _mappers);
            result.AddMapper(_mappers);
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

        private static IEmitterType StoreTargetObjectMember(MappingMember member, IEmitterType targetObject, IEmitterType convertedMember)
        {
            IEmitterType result = null;
            member.Target
                  .ToOption()
                  .Match(x => x.IsField(), x => result = StoreFiled((FieldInfo)x, targetObject, convertedMember))
                  .Match(x => x.IsProperty(), x => result = StoreProperty((PropertyInfo)x, targetObject, convertedMember));
            return result;
        }

        private IEmitter Build(MappingMember member)
        {
            IEmitterType sourceObject = EmitterLocal.Load(_config.LocalSource);

            IEmitterType memberValue = LoadSourceObjectMember(member, sourceObject);

            IEmitterType convertedMember = ConvertMember(member, memberValue);

            IEmitterType targetObject = EmitterLocal.LoadAddress(_config.LocalTarget);

            IEmitter result = StoreTargetObjectMember(member, targetObject, convertedMember);
            return result;
        }

        private IEmitterType ConvertComplexType(ComplexMappingMember member)
        {
            CollectionMapper mapper = CollectionMapper.Create(_config.Assembly, member);
            MapperCacheItem mapperCacheItem = _mappers.Add(member.TypePair, mapper);
            return CallMapMethod(mapperCacheItem);
        }

        private IEmitterType CallMapMethod(MapperCacheItem mapperCacheItem)
        {
            Type mapperType = typeof(Mapper);
            MethodInfo mapMethod = mapperType.GetMethod(Mapper.MapMethodName, BindingFlags.Public);
            IEmitterType mappers = EmitterField.Load(EmitterThis.Load(mapperType), mapperType.GetField("_mappers"));
            IEmitterType mapper = EmitterArray.Load(mappers, mapperCacheItem.Id);
            IEmitterType result = EmitterMethod.Call(mapMethod, mapper, EmitterLocal.Load(_config.LocalSource), EmitterLocal.Load(_config.LocalTarget));
            return result;
        }

        private IEmitterType ConvertMember(MappingMember member, IEmitterType memberValue)
        {
            if (member is PrimitiveMappingMember)
            {
                return ConvertPrimitiveType((PrimitiveMappingMember)member, memberValue);
            }
            return ConvertComplexType((ComplexMappingMember)member);
        }

        private IEmitterType ConvertPrimitiveType(PrimitiveMappingMember member, IEmitterType memberValue)
        {
            MethodInfo converter = PrimitiveTypeConverter.GetConverter(member.TypePair);
            IEmitterType result = EmitterMethod.Call(converter, null, memberValue);
            return result;
        }

        private IEmitterType LoadField(IEmitterType source, FieldInfo field)
        {
            return EmitterField.Load(source, field);
        }

        private IEmitterType LoadProperty(IEmitterType source, PropertyInfo property)
        {
            return EmitterProperty.Load(source, property);
        }

        private IEmitterType LoadSourceObjectMember(MappingMember member, IEmitterType sourceObject)
        {
            IEmitterType result = null;
            member.Source
                  .ToOption()
                  .Match(x => x.IsField(), x => result = LoadField(sourceObject, (FieldInfo)x))
                  .Match(x => x.IsProperty(), x => result = LoadProperty(sourceObject, (PropertyInfo)x));
            return result;
        }


        private sealed class MemberMapperConfig : IMemberMapperConfig
        {
            public IDynamicAssembly Assembly { get; set; }
            public CodeGenerator CodeGenerator { get; set; }
            public LocalBuilder LocalSource { get; set; }
            public LocalBuilder LocalTarget { get; set; }

            public IMemberMapperConfig Config(Action<IMemberMapperConfig> action)
            {
                action(this);
                return this;
            }

            public MemberMapper Create()
            {
                Validate();
                return new MemberMapper(this);
            }

            private void Validate()
            {
                var nullCheck = new List<object>
                {
                    LocalSource, LocalTarget, CodeGenerator, Assembly
                };

                if (nullCheck.Any(x => x.IsNull()))
                {
                    throw new ConfigurationErrorsException();
                }
            }
        }
    }
}
