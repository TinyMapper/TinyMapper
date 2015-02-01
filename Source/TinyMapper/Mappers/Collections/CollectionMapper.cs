using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TinyMappers.CodeGenerators;
using TinyMappers.CodeGenerators.Emitters;
using TinyMappers.Core;
using TinyMappers.DataStructures;
using TinyMappers.Extensions;
using TinyMappers.Reflection;
using TinyMappers.TypeConverters;

namespace TinyMappers.Mappers.Collections
{
    internal sealed class CollectionMapperBuilder
    {
        private const string MapperNamePrefix = "TinyCollection";
        private const MethodAttributes OverrideProtected = MethodAttributes.Family | MethodAttributes.Virtual;
        private const string ConvertItemMethod = "ConvertItem";
        private const string EnumerableToListMethod = "EnumerableToList";
        private const string EnumerableToListTemplateMethod = "EnumerableToListTemplate";

        public static Mapper Create(IDynamicAssembly assembly, TypePair typePair)
        {
            Type parentType = typeof(CollectionMapper<,>).MakeGenericType(typePair.Source, typePair.Target);
            TypeBuilder typeBuilder = assembly.DefineType(GetMapperName(), parentType);
            if (IsIEnumerableOfToList(typePair))
            {
                EmitEnumerableToList(parentType, typeBuilder, typePair);
            }
            var result = (Mapper)Activator.CreateInstance(typeBuilder.CreateType());
            return result;
        }

        private static void EmitConvertItem(TypeBuilder typeBuilder, TypePair typePair)
        {
            var methodBuilder = typeBuilder.DefineMethod(ConvertItemMethod, OverrideProtected, Types.Object, new[] { Types.Object });

            IEmitterType converter;

            if (PrimitiveTypeConverter.IsSupported(typePair))
            {
                MethodInfo converterMethod = PrimitiveTypeConverter.GetConverter(typePair);
                converter = EmitMethod.Call(converterMethod, null, EmitArgument.Load(typePair.Source, 1));
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

        private static string GetMapperName()
        {
            string random = Guid.NewGuid().ToString("N");
            return string.Format("{0}_{1}", MapperNamePrefix, random);
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


    internal abstract class CollectionMapper<TSource, TTarget> : MapperOf<TSource, TTarget>
    {
        protected override TTarget MapCore(TSource source, TTarget target)
        {
            return EnumerableToList((IEnumerable)source);
        }

        protected virtual object ConvertItem(object item)
        {
            throw new NotImplementedException();
        }

        protected virtual TTarget EnumerableToList(IEnumerable value)
        {
            throw new NotImplementedException();
        }

        protected List<TTargetItem> EnumerableToListTemplate<TTargetItem>(IEnumerable source)
        {
            var result = new List<TTargetItem>();
            foreach (object item in source)
            {
                result.Add((TTargetItem)ConvertItem(item));
            }
            return result;
        }
    }
}
