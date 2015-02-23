using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Nelibur.ObjectMapper.CodeGenerators;
using Nelibur.ObjectMapper.CodeGenerators.Emitters;
using Nelibur.ObjectMapper.Core;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;
using Nelibur.ObjectMapper.Mappers.Caches;

namespace Nelibur.ObjectMapper.Mappers.Collections
{
    internal sealed class CollectionMapperBuilder : MapperBuilder
    {
        private const string ConvertItemMethod = "ConvertItem";
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

        protected override Mapper CreateCore(TypePair typePair)
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
            var result = (Mapper)Activator.CreateInstance(typeBuilder.CreateType());
            result.AddMappers(_mapperCache.Mappers);
            return result;
        }

        protected override bool IsSupportedCore(TypePair typePair)
        {
            return typePair.IsEnumerableTypes;
        }

        private static Type GetCollectionItemType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }
            else if (type.IsListOf())
            {
                return type.GetGenericArguments().First();
            }
            throw new NotSupportedException();
        }

        private static bool IsIEnumerableToList(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsListOf();
        }

        private MapperCacheItem CreateMapperCacheItem(TypePair typePair)
        {
            MapperBuilder mapperBuilder = GetMapperBuilder(typePair);
            Mapper mapper = mapperBuilder.Create(typePair);
            MapperCacheItem mapperCacheItem = _mapperCache.Add(typePair, mapper);
            return mapperCacheItem;
        }

        private void EmitConvertItem(TypeBuilder typeBuilder, TypePair typePair)
        {
            MapperCacheItem mapperCacheItem = CreateMapperCacheItem(typePair);

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(ConvertItemMethod, OverrideProtected, Types.Object, new[] { Types.Object });

            IEmitterType sourceMemeber = EmitArgument.Load(Types.Object, 1);
            IEmitterType targetMember = EmitNull.Load();

            IEmitterType callMapMethod = mapperCacheItem.EmitMapMethod(sourceMemeber, targetMember);

            EmitReturn.Return(callMapMethod).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
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
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodName, OverrideProtected, typePair.Target, new[] { Types.IEnumerable });

            Type sourceItemType = GetCollectionItemType(typePair.Source);
            Type targetItemType = GetCollectionItemType(typePair.Target);

            EmitConvertItem(typeBuilder, new TypePair(sourceItemType, targetItemType));

            MethodInfo methodTemplate = parentType.GetGenericMethod(templateMethodName, targetItemType);

            IEmitterType returnValue = EmitMethod.Call(methodTemplate, EmitThis.Load(parentType), EmitArgument.Load(Types.IEnumerable, 1));
            EmitReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }

        private bool IsIEnumerableToArray(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsArray;
        }
    }
}
