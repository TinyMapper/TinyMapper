using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using Nelibur.ObjectMapper.CodeGenerators;
using Nelibur.ObjectMapper.CodeGenerators.Emitters;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;
using Nelibur.ObjectMapper.Mappers.Caches;

namespace Nelibur.ObjectMapper.Mappers.Collections
{
    internal sealed class CollectionMapperBuilder : MapperBuilder
    {
        private const string ConvertItemMethod = "ConvertItem";
        private const string ConvertItemKeyMethod = "ConvertItemKey";
        private const string DictionaryToDictionaryMethod = "DictionaryToDictionary";
        private const string DictionaryToDictionaryTemplateMethod = "DictionaryToDictionaryTemplate";
        private const string EnumerableToArrayMethod = "EnumerableToArray";
        private const string EnumerableToArrayTemplateMethod = "EnumerableToArrayTemplate";
        private const string EnumerableToListMethod = "EnumerableToList";
        private const string EnumerableToListTemplateMethod = "EnumerableToListTemplate";
        private readonly MapperCache _mapperCache = new MapperCache();

        public CollectionMapperBuilder(IMapperBuilderConfig config) : base(config)
        {
        }

        protected override string ScopeName
        {
            get { return "CollectionMappers"; }
        }

        protected override Mapper BuildCore(TypePair typePair)
        {
            Type parentType = typeof(CollectionMapper<,>).MakeGenericType(typePair.Source, typePair.Target);
            TypeBuilder typeBuilder = _assembly.DefineType(GetMapperFullName(), parentType);
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
            var result = (Mapper)Activator.CreateInstance(typeBuilder.CreateType());
            result.AddMappers(_mapperCache.Mappers);
            return result;
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

        private MapperCacheItem CreateMapperCacheItem(TypePair typePair)
        {
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

        private void EmitEnumerableToArray(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            EmitEnumerableToTarget(parentType, typeBuilder, typePair, EnumerableToArrayMethod, EnumerableToArrayTemplateMethod);
        }

        private void EmitEnumerableToList(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            EmitEnumerableToTarget(parentType, typeBuilder, typePair, EnumerableToListMethod, EnumerableToListTemplateMethod);
        }

        private void EmitEnumerableToTarget(Type parentType, TypeBuilder typeBuilder, TypePair typePair,
            string methodName, string templateMethodName)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodName, OverrideProtected, typePair.Target, new[] { typeof(IEnumerable) });

            Type sourceItemType = typePair.Source.GetCollectionItemType();
            Type targetItemType = typePair.Target.GetCollectionItemType();

            EmitConvertItem(typeBuilder, new TypePair(sourceItemType, targetItemType));

            MethodInfo methodTemplate = parentType.GetGenericMethod(templateMethodName, targetItemType);

            IEmitterType returnValue = EmitMethod.Call(methodTemplate, EmitThis.Load(parentType), EmitArgument.Load(typeof(IEnumerable), 1));
            EmitReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }

        private void EmitDictionaryToTarget(Type parentType, TypeBuilder typeBuilder, TypePair typePair,
            string methodName, string templateMethodName)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodName, OverrideProtected, typePair.Target, new[] { typeof(IEnumerable) });

            var sourceTypes = typePair.Source.GetDictionaryItemTypes();
            var targetTypes = typePair.Target.GetDictionaryItemTypes();

            EmitConvertItem(typeBuilder, new TypePair(sourceTypes.Item1, targetTypes.Item1), ConvertItemKeyMethod);
            EmitConvertItem(typeBuilder, new TypePair(sourceTypes.Item2, targetTypes.Item2));

            MethodInfo methodTemplate = parentType.GetGenericMethod(templateMethodName, new[] { targetTypes.Item1, targetTypes.Item2 });

            IEmitterType returnValue = EmitMethod.Call(methodTemplate, EmitThis.Load(parentType), EmitArgument.Load(typeof(IEnumerable), 1));
            EmitReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }
    }
}
