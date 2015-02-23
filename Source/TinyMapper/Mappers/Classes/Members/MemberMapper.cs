using System;
using System.Collections.Generic;
using System.Reflection;
using Nelibur.ObjectMapper.CodeGenerators.Emitters;
using Nelibur.ObjectMapper.Core.Extensions;
using Nelibur.ObjectMapper.Mappers.Caches;
using Nelibur.ObjectMapper.Mappers.MappingMembers;

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

        public MemberEmitterDescription Build(List<MappingMember> members)
        {
            var emitter = new EmitComposite();
            members.ForEach(x => emitter.Add(Build(x)));
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

        private IEmitter Build(MappingMember member)
        {
            IEmitterType sourceObject = EmitArgument.Load(member.Source.GetMemberType(), 1);
            IEmitterType targetObject = EmitArgument.Load(member.Target.GetMemberType(), 2);

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
            IEmitterType mappers = EmitField.Load(EmitThis.Load(mapperType), mappersField);
            IEmitterType mapper = EmitArray.Load(mappers, mapperCacheItem.Id);
            IEmitterType result = EmitMethod.Call(mapMethod, mapper, sourceMemeber, targetMember);
            return result;
        }

        /// <summary>
        /// Converts the member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="sourceMemeber">The source memeber.</param>
        /// <param name="targetMember">The target member.</param>
        /// <returns></returns>
        private IEmitterType ConvertMember(MappingMember member, IEmitterType sourceMemeber, IEmitterType targetMember)
        {
            if (member.TypePair.IsDeepCloneable)
            {
                return sourceMemeber;
            }

            MapperBuilder mapperBuilder = _config.GetMapperBuilder(member.TypePair);
            Mapper mapper = mapperBuilder.Create(member.TypePair);
            MapperCacheItem mapperCacheItem = _mapperCache.Add(member.TypePair, mapper);
            return CallMapMethod(mapperCacheItem, sourceMemeber, targetMember);

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
