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

namespace TinyMappers.Mappers.Collections
{
    internal sealed class CollectionMapperBuilder
    {
        private const BindingFlags InstanceNonPublic = BindingFlags.Instance | BindingFlags.NonPublic;
        private const string MapperNamePrefix = "TinyCollection";
        private const MethodAttributes OverrideProtected = MethodAttributes.Family | MethodAttributes.Virtual;
        private const string EnumerableOfToListMethod = "EnumerableOfToList";
        private const string EnumerableOfToListTemplateMethod = "EnumerableOfToListTemplate";
        private const string MapCoreMethod = "MapCore";

        public static Mapper Create(IDynamicAssembly assembly, TypePair typePair)
        {
            Type parentType = typeof(CollectionMapper<,>).MakeGenericType(typePair.Source, typePair.Target);
            TypeBuilder typeBuilder = assembly.DefineType(GetMapperName(), parentType);
            MethodBuilder mapCoreBuilder = EmitMapCore(typeBuilder, typePair);
            IEmitterType mapCoreBody;
            if (IsIEnumerableOfToList(typePair))
            {
                mapCoreBody = EmitEnumerableOfToList(parentType, typeBuilder, typePair);
            }
            else
            {
                mapCoreBody = EmitNull.Load();
            }
            EmitReturn.Return(mapCoreBody).Emit(new CodeGenerator(mapCoreBuilder.GetILGenerator()));

            var result = (Mapper)Activator.CreateInstance(typeBuilder.CreateType());
            return result;
        }

        private static IEmitterType EmitEnumerableOfToList(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            Type sourceItemType = GetCollectionItemType(typePair.Source);
            Type targetItemType = GetCollectionItemType(typePair.Target);

            MethodBuilder methodBuilder = DefineEnumerableOfToList(typeBuilder, typePair);

            Type sourceType = Types.IEnumerableOf.MakeGenericType(sourceItemType);

            MethodInfo methodTemplate = parentType.GetGenericMethod(EnumerableOfToListTemplateMethod, sourceItemType, targetItemType);

            IEmitterType returnValue = EmitMethod.Call(methodTemplate, EmitThis.Load(parentType), EmitArgument.Load(Types.IEnumerable, 1));
            EmitReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));

            MethodInfo method = parentType.GetGenericMethod(EnumerableOfToListMethod, sourceItemType);

            var result = EmitMethod.Call(method, EmitThis.Load(parentType), EmitArgument.Load(sourceType, 1));
            return result;
        }

        private static MethodBuilder DefineEnumerableOfToList(TypeBuilder typeBuilder, TypePair typePair)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(EnumerableOfToListMethod, OverrideProtected);
            GenericTypeParameterBuilder sourceItem = methodBuilder.DefineGenericParameters("TSourceItem")[0];
            Type sourceType = Types.IEnumerableOf.MakeGenericType(sourceItem);
            methodBuilder.SetParameters(sourceType);
            methodBuilder.SetReturnType(typePair.Target);
            return methodBuilder;
        }

        private static MethodBuilder EmitMapCore(TypeBuilder typeBuilder, TypePair typePair)
        {
            var methodArgs = new[] { typePair.Source, typePair.Target };
            var methodBuilder = typeBuilder.DefineMethod(MapCoreMethod, OverrideProtected, typePair.Target, methodArgs);
            return methodBuilder;
        }

        private static void EmitToList(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("EnumerableToList", OverrideProtected, typePair.Target, new[] { Types.IEnumerable });

            Type targetItemType = GetCollectionItemType(typePair.Target);

            MethodInfo methodTemplate = parentType
                .GetMethod("EnumerableToListTemplate", InstanceNonPublic)
                .MakeGenericMethod(targetItemType);

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
            throw new NotImplementedException();
        }

        protected virtual TTargetItem ConvertItem<TSourceItem, TTargetItem>(TSourceItem item)
        {
            return (TTargetItem)((object)item);
            //            throw new NotImplementedException();
        }

        protected virtual TTarget EnumerableOfToList<TSourceItem>(IEnumerable<TSourceItem> value)
        {
            throw new NotImplementedException();
        }

        protected List<TTargetItem> EnumerableOfToListTemplate<TSourceItem, TTargetItem>(IEnumerable<TSourceItem> source)
        {
            var result = new List<TTargetItem>();
            foreach (TSourceItem item in source)
            {
                result.Add(ConvertItem<TSourceItem, TTargetItem>(item));
            }
            return result;
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
                result.Add((TTargetItem)item);
            }
            return result;
        }
    }
}
