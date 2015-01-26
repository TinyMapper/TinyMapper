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
            IEmitterType targetObject = EmitterLocal.LoadAddress(_config.LocalTarget);

            IEmitterType sourceMember = LoadMember(member.Source, sourceObject);
            IEmitterType targetMember = LoadMember(member.Target, targetObject);

            IEmitterType convertedMember = ConvertMember(member, sourceMember, targetMember);

            IEmitter result = StoreTargetObjectMember(member, targetObject, convertedMember);
            return result;
        }

        private IEmitterType CallMapMethod(MapperCacheItem mapperCacheItem, IEmitterType sourceMemeber, IEmitterType targetMember)
        {
            Type mapperType = typeof(Mapper);
            MethodInfo mapMethod = mapperType.GetMethod(Mapper.MapMethodName, BindingFlags.Instance | BindingFlags.Public);
            FieldInfo mappersField = mapperType.GetField(Mapper.MappersFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            IEmitterType mappers = EmitterField.Load(EmitterThis.Load(mapperType), mappersField);
            IEmitterType mapper = EmitterArray.Load(mappers, mapperCacheItem.Id);
            IEmitterType result = EmitterMethod.Call(mapMethod, mapper, sourceMemeber, targetMember);
            return result;
        }

        private IEmitterType ConvertComplexType(ComplexMappingMember member, IEmitterType sourceMemeber, IEmitterType targetMember)
        {
            CollectionMapper mapper = CollectionMapper.Create(_config.Assembly, member);
            MapperCacheItem mapperCacheItem = _mappers.Add(member.TypePair, mapper);
            return CallMapMethod(mapperCacheItem, sourceMemeber, targetMember);
        }

        private IEmitterType ConvertMember(MappingMember member, IEmitterType sourceMemeber, IEmitterType targetMember)
        {
            IEmitterType result = null;
            member.ToOption()
                  .MatchType<PrimitiveMappingMember>(x => result = ConvertPrimitiveType(x, sourceMemeber))
                  .MatchType<ComplexMappingMember>(x => result = ConvertComplexType(x, sourceMemeber, targetMember));
            return result;
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

        private IEmitterType LoadMember(MemberInfo member, IEmitterType sourceObject)
        {
            IEmitterType result = null;
            member.ToOption()
                  .Match(x => x.IsField(), x => result = LoadField(sourceObject, (FieldInfo)x))
                  .Match(x => x.IsProperty(), x => result = LoadProperty(sourceObject, (PropertyInfo)x));
            return result;
        }

        private IEmitterType LoadProperty(IEmitterType source, PropertyInfo property)
        {
            return EmitterProperty.Load(source, property);
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
