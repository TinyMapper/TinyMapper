using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Nelibur.ObjectMapper.CodeGenerators;
using Nelibur.ObjectMapper.CodeGenerators.Emitters;
using Nelibur.ObjectMapper.Core;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;
using Nelibur.ObjectMapper.Mappers.Caches;
using Nelibur.ObjectMapper.Mappers.Classes.Members;

namespace Nelibur.ObjectMapper.Mappers.Collections
{
    internal sealed class CollectionMapperBuilder : MapperBuilder
    {
        private readonly MapperCache _mapperCache;
        private const string ConvertItemKeyMethod = "ConvertItemKey";
        private const string ConvertItemMethod = "ConvertItem";
        private const string DictionaryToDictionaryMethod = "DictionaryToDictionary";
        private const string DictionaryToDictionaryTemplateMethod = "DictionaryToDictionaryTemplate";
        private const string EnumerableToArrayMethod = "EnumerableToArray";
        private const string EnumerableToArrayTemplateMethod = "EnumerableToArrayTemplate";
        private const string EnumerableToListMethod = "EnumerableToList";
        private const string EnumerableToListTemplateMethod = "EnumerableToListTemplate";
        private const string EnumerableOfDeepCloneableToListTemplateMethod = "EnumerableOfDeepCloneableToListTemplate";

        public CollectionMapperBuilder(MapperCache mapperCache, IMapperBuilderConfig config) : base(config)
        {
            _mapperCache = mapperCache;
        }

        protected override string ScopeName => "CollectionMappers";

        protected override Mapper BuildCore(TypePair typePair)
        {
            Type parentType = typeof(CollectionMapper<,>).MakeGenericType(typePair.Source, typePair.Target);
            TypeBuilder typeBuilder = _assembly.DefineType(GetMapperFullName(), parentType);

            _mapperCache.AddStub(typePair);

            if (IsIEnumerableToList(typePair))
            {
                EmitEnumerableToList(parentType, typeBuilder, typePair);
            }
            else if (IsIEnumerableToArray(typePair))
            {
                EmitEnumerableToArray(parentType, typeBuilder, typePair);
            }
            else if (IsDictionaryToDictionary(typePair))
            {
                EmitDictionaryToDictionary(parentType, typeBuilder, typePair);
            }
            else if (IsEnumerableToEnumerable(typePair))
            {
                EmitEnumerableToEnumerable(parentType, typeBuilder, typePair);
            }

            var rootMapper = (Mapper)Activator.CreateInstance(Helpers.CreateType(typeBuilder));

            _mapperCache.ReplaceStub(typePair, rootMapper);
            rootMapper.AddMappers(_mapperCache.Mappers);

            return rootMapper;
        }

        protected override Mapper BuildCore(TypePair parentTypePair, MappingMember mappingMember)
        {
            return BuildCore(mappingMember.TypePair);
        }

        protected override bool IsSupportedCore(TypePair typePair)
        {
            return typePair.IsEnumerableTypes;
        }

        private static bool IsDictionaryToDictionary(TypePair typePair)
        {
            return typePair.Source.IsDictionaryOf() && typePair.Target.IsDictionaryOf();
        }

        private static bool IsIEnumerableToArray(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsArray;
        }

        private static bool IsIEnumerableToList(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsListOf();
        }

        private bool IsEnumerableToEnumerable(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsIEnumerable();
        }

        private MapperCacheItem CreateMapperCacheItem(TypePair typePair)
        {
            var mapperCacheItemOption = _mapperCache.Get(typePair);
            if (mapperCacheItemOption.HasValue)
            {
                return mapperCacheItemOption.Value;
            }

            MapperBuilder mapperBuilder = GetMapperBuilder(typePair);
            Mapper mapper = mapperBuilder.Build(typePair);
            MapperCacheItem mapperCacheItem = _mapperCache.Add(typePair, mapper);
            return mapperCacheItem;
        }

        private void EmitConvertItem(TypeBuilder typeBuilder, TypePair typePair, string methodName = ConvertItemMethod)
        {
            MapperCacheItem mapperCacheItem = CreateMapperCacheItem(typePair);

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodName, OverrideProtected, typeof(object), new[] { typeof(object) });

            IEmitterType sourceMemeber = EmitArgument.Load(typeof(object), 1);
            IEmitterType targetMember = EmitNull.Load();

            IEmitterType callMapMethod = mapperCacheItem.EmitMapMethod(sourceMemeber, targetMember);

            EmitReturn.Return(callMapMethod).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }

        private void EmitDictionaryToDictionary(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            EmitDictionaryToTarget(parentType, typeBuilder, typePair, DictionaryToDictionaryMethod, DictionaryToDictionaryTemplateMethod);
        }

        private void EmitDictionaryToTarget(
            Type parentType,
            TypeBuilder typeBuilder,
            TypePair typePair,
            string methodName,
            string templateMethodName)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodName, OverrideProtected, typePair.Target, new[] { typeof(IEnumerable) });

            KeyValuePair<Type, Type> sourceTypes = typePair.Source.GetDictionaryItemTypes();
            KeyValuePair<Type, Type> targetTypes = typePair.Target.GetDictionaryItemTypes();

            EmitConvertItem(typeBuilder, new TypePair(sourceTypes.Key, targetTypes.Key), ConvertItemKeyMethod);
            EmitConvertItem(typeBuilder, new TypePair(sourceTypes.Value, targetTypes.Value));

            var arguments = new[] { sourceTypes.Key, sourceTypes.Value, targetTypes.Key, targetTypes.Value };
            MethodInfo methodTemplate = parentType.GetGenericMethod(templateMethodName, arguments);

            IEmitterType returnValue = EmitMethod.Call(methodTemplate, EmitThis.Load(parentType), EmitArgument.Load(typeof(IEnumerable), 1));
            EmitReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }

        private void EmitEnumerableToArray(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            var collectionItemTypePair = GetCollectionItemTypePair(typePair);

            EmitEnumerableToTarget(parentType, typeBuilder, typePair, collectionItemTypePair, EnumerableToArrayMethod, EnumerableToArrayTemplateMethod);
        }

        private void EmitEnumerableToList(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            var collectionItemTypePair = GetCollectionItemTypePair(typePair);
            var templateMethod = collectionItemTypePair.IsDeepCloneable ? EnumerableOfDeepCloneableToListTemplateMethod : EnumerableToListTemplateMethod;

            EmitEnumerableToTarget(parentType, typeBuilder, typePair, collectionItemTypePair, EnumerableToListMethod, templateMethod);
        }

        private void EmitEnumerableToEnumerable(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            var collectionItemTypePair = GetCollectionItemTypePair(typePair);
            var templateMethod = collectionItemTypePair.IsDeepCloneable ? EnumerableOfDeepCloneableToListTemplateMethod : EnumerableToListTemplateMethod;

            EmitEnumerableToTarget(parentType, typeBuilder, typePair, collectionItemTypePair, EnumerableToListMethod, templateMethod);
        }

        private static TypePair GetCollectionItemTypePair(TypePair typePair)
        {
            Type sourceItemType = typePair.Source.GetCollectionItemType();
            Type targetItemType = typePair.Target.GetCollectionItemType();

            return new TypePair(sourceItemType, targetItemType);
        }

        private void EmitEnumerableToTarget(
            Type parentType,
            TypeBuilder typeBuilder,
            TypePair typePair,
            TypePair collectionItemTypePair,
            string methodName,
            string templateMethodName)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodName, OverrideProtected, typePair.Target, new[] { typeof(IEnumerable) });

            EmitConvertItem(typeBuilder, collectionItemTypePair);

            MethodInfo methodTemplate = parentType.GetGenericMethod(templateMethodName, collectionItemTypePair.Target);

            IEmitterType returnValue = EmitMethod.Call(methodTemplate, EmitThis.Load(parentType), EmitArgument.Load(typeof(IEnumerable), 1));
            EmitReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }
    }
}
