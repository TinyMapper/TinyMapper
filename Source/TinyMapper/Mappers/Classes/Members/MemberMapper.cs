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
        private readonly MapperCache _mapperCache;


        public MemberMapper(MapperCache mapperCache, IMapperBuilderConfig config)
        {
            _mapperCache = mapperCache;
            _config = config;
        }

        public MemberEmitterDescription Build(TypePair parentTypePair, List<MappingMemberPath> members)
        {
            var emitComposite = new EmitComposite();
            foreach (var path in members)
            {
                IEmitter emitter = Build(parentTypePair, path);
                emitComposite.Add(emitter);
            }
            var result = new MemberEmitterDescription(emitComposite, _mapperCache);
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

        private IEmitter Build(TypePair parentTypePair, MappingMemberPath memberPath)
        {

            if (memberPath.OneLevelTarget)
            {
                var sourceObject = EmitArgument.Load(memberPath.TypePair.Source, 1);
                var targetObject = EmitArgument.Load(memberPath.TypePair.Target, 2);

                var sourceMember = LoadMember(memberPath.Source, sourceObject, memberPath.Source.Count);
                var targetMember = LoadMember(memberPath.Target, targetObject, memberPath.Target.Count);

                IEmitterType convertedMember = ConvertMember(parentTypePair, memberPath.Tail, sourceMember, targetMember);

                IEmitter result = StoreTargetObjectMember(memberPath.Tail, targetObject, convertedMember);
                return result;
            }
            else
            {
                var targetObject = EmitArgument.Load(memberPath.Head.TypePair.Target, 2);
                var targetMember = LoadMember(memberPath.Target, targetObject, memberPath.Target.Count - 1);

                var sourceObject = EmitArgument.Load(memberPath.Head.TypePair.Source, 1);
                var sourceMember = LoadMember(memberPath.Source, sourceObject, memberPath.Source.Count);

                IEmitterType convertedMember = ConvertMember(parentTypePair, memberPath.Tail, sourceMember, targetMember);

                IEmitter result = StoreTargetObjectMember(memberPath.Tail, targetMember, convertedMember);
                return result;
            }
        }

        private IEmitterType ConvertMember(TypePair parentTypePair, MappingMember member, IEmitterType sourceMemeber, IEmitterType targetMember)
        {
            //            if (member.TypePair.IsDeepCloneable && _config.GetBindingConfig(parentTypePair).HasNoValue)
            if (member.TypePair.IsDeepCloneable)
            {
                return sourceMemeber;
            }

            MapperCacheItem mapperCacheItem = CreateMapperCacheItem(parentTypePair, member);

            IEmitterType result = mapperCacheItem.EmitMapMethod(sourceMemeber, targetMember);
            return result;
        }

        private MapperCacheItem CreateMapperCacheItem(TypePair parentTypePair, MappingMember mappingMember)
        {
            var mapperCacheItemOption = _mapperCache.Get(mappingMember.TypePair);
            if (mapperCacheItemOption.HasValue)
            {
                return mapperCacheItemOption.Value;
            }

            MapperBuilder mapperBuilder = _config.GetMapperBuilder(parentTypePair, mappingMember);
            Mapper mapper = mapperBuilder.Build(parentTypePair, mappingMember);
            MapperCacheItem mapperCacheItem = _mapperCache.Add(mappingMember.TypePair, mapper);
            return mapperCacheItem;
        }

        private IEmitterType LoadField(IEmitterType source, FieldInfo field)
        {
            return EmitField.Load(source, field);
        }

        private IEmitterType LoadMember(List<MemberInfo> members, IEmitterType sourceObject, int loadLevel)
        {
            IEmitterType dummySource = sourceObject;
            if (members.Count == 1)
            {
                return LoadMember(members[0], dummySource);
            }
            for (int i = 0; i < loadLevel; i++)
            {
                dummySource = LoadMember(members[i], dummySource);
            }
            return dummySource;
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
