using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Nelibur.ObjectMapper.CodeGenerators;
using Nelibur.ObjectMapper.CodeGenerators.Emitters;
using Nelibur.ObjectMapper.Core;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;
using Nelibur.ObjectMapper.Mappers.MappingMembers;
using Nelibur.ObjectMapper.TypeConverters;

namespace Nelibur.ObjectMapper.Mappers.Collections
{
    internal sealed class CollectionMapperBuilder : MapperBuilder
    {
        private const string ConvertItemMethod = "ConvertItem";
        private const string EnumerableToListMethod = "EnumerableToList";
        private const string EnumerableToListTemplateMethod = "EnumerableToListTemplate";

        public CollectionMapperBuilder(IMapperBuilderConfig config) : base(config)
        {
        }

        protected override string ScopeName
        {
            get { return "CollectionMappers"; }
        }

        public Mapper Create(ComplexMappingMember member)
        {
            TypePair typePair = member.TypePair;
            Type parentType = typeof(CollectionMapper<,>).MakeGenericType(typePair.Source, typePair.Target);
            TypeBuilder typeBuilder = _assembly.DefineType(GetMapperFullName(), parentType);
            if (IsIEnumerableOfToList(typePair))
            {
                EmitEnumerableToList(parentType, typeBuilder, typePair);
            }
            var result = (Mapper)Activator.CreateInstance(typeBuilder.CreateType());
            return result;
        }

        protected override bool IsSupportedCore(TypePair typePair)
        {
            return typePair.IsEnumerableTypes;
        }

        private static void EmitConvertItem(TypeBuilder typeBuilder, TypePair typePair)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(ConvertItemMethod, OverrideProtected, Types.Object, new[] { Types.Object });

            IEmitterType converter;

            if (PrimitiveTypeConverter.IsSupported(typePair))
            {
                MethodInfo converterMethod = PrimitiveTypeConverter.GetConverter(typePair);
                converter = EmitMethod.CallStatic(converterMethod, EmitArgument.Load(typePair.Source, 1));
            }
            else
            {
                throw new NotSupportedException();
            }
            EmitReturn.Return(converter).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }

        private static void EmitEnumerableToList(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(EnumerableToListMethod, OverrideProtected, typePair.Target, new[] { Types.IEnumerable });

            Type sourceItemType = GetCollectionItemType(typePair.Source);
            Type targetItemType = GetCollectionItemType(typePair.Target);

            EmitConvertItem(typeBuilder, new TypePair(sourceItemType, targetItemType));

            MethodInfo methodTemplate = parentType.GetGenericMethod(EnumerableToListTemplateMethod, targetItemType);

            IEmitterType returnValue = EmitMethod.Call(methodTemplate, EmitThis.Load(parentType), EmitArgument.Load(Types.IEnumerable, 1));
            EmitReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }

        private static Type GetCollectionItemType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }
            else if (IsList(type))
            {
                return type.GetGenericArguments().First();
            }
            throw new NotSupportedException();
        }

        private static bool IsIEnumerableOf(Type type)
        {
            return type.GetInterfaces()
                       .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == Types.IEnumerableOf);
        }

        private static bool IsIEnumerableOfToList(TypePair typePair)
        {
            return IsList(typePair.Target) && IsIEnumerableOf(typePair.Source);
        }

        private static bool IsList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}
