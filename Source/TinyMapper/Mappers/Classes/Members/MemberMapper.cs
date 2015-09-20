using System;
using System.Collections.Generic;
using System.Reflection;
using Nelibur.ObjectMapper.CodeGenerators.Emitters;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;
using Nelibur.ObjectMapper.Mappers.Caches;

namespace Nelibur.ObjectMapper.Mappers.Classes.Members
{
    internal sealed class MemberMapper
    {
        private readonly IMapperBuilderConfig _config;
        private readonly MapperCache _mapperCache = new MapperCache();

        public MemberMapper(IMapperBuilderConfig config)
        {
            _config = config;
        }

        public MemberEmitterDescription Build(TypePair parentTypePair, List<MappingMember> members)
        {
            var emitter = new EmitComposite();
            members.ForEach(x => emitter.Add(Build(parentTypePair, x)));
            var result = new MemberEmitterDescription(emitter, _mapperCache);
            result.AddMapper(_mapperCache);
            return result;
        }

        private static IEmitterType StoreFiled(FieldInfo field, IEmitterType targetObject, IEmitterType value)
        {
            return EmitField.Store(field, targetObject, value);
        }

        private static IEmitterType StoreProperty(PropertyInfo property, IEmitterType targetObject, IEmitterType value)
        {
            return EmitProperty.Store(property, targetObject, value);
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

        private IEmitter Build(TypePair parentTypePair, MappingMember member)
        {
            IEmitterType sourceObject = EmitArgument.Load(member.TypePair.Source, 1);
            IEmitterType targetObject = EmitArgument.Load(member.TypePair.Target, 2);

            IEmitterType sourceMember = LoadMember(member.Source, sourceObject);
            IEmitterType targetMember = LoadMember(member.Target, targetObject);

            IEmitterType convertedMember = ConvertMember(parentTypePair, member, sourceMember, targetMember);

            IEmitter result = StoreTargetObjectMember(member, targetObject, convertedMember);
            return result;
        }

        private IEmitterType ConvertMember(TypePair parentTypePair, MappingMember member, IEmitterType sourceMemeber, IEmitterType targetMember)
        {
            if (member.TypePair.IsDeepCloneable && _config.GetBindingConfig(parentTypePair).HasNoValue)
            {
                return sourceMemeber;
            }

            MapperCacheItem mapperCacheItem = CreateMapperCacheItem(parentTypePair, member);

            IEmitterType result = mapperCacheItem.EmitMapMethod(sourceMemeber, targetMember);
            return result;
        }

        private MapperCacheItem CreateMapperCacheItem(TypePair parentTypePair, MappingMember mappingMember)
        {
            MapperBuilder mapperBuilder = _config.GetMapperBuilder(parentTypePair, mappingMember);
            Mapper mapper = mapperBuilder.Build(parentTypePair, mappingMember);
            MapperCacheItem mapperCacheItem = _mapperCache.Add(mappingMember.TypePair, mapper);

            return mapperCacheItem;
        }

        private IEmitterType LoadField(IEmitterType source, FieldInfo field)
        {
            return EmitField.Load(source, field);
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
            return EmitProperty.Load(source, property);
        }
    }
}
