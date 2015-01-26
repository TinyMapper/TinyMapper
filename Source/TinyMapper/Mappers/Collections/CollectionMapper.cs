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
using TinyMappers.Mappers.Types1.Members;
using TinyMappers.Reflection;

namespace TinyMappers.Mappers.Collections
{
    internal abstract class CollectionMapper : Mapper
    {
        private const BindingFlags InstanceNonPublic = BindingFlags.Instance | BindingFlags.NonPublic;
        private const string MapperNamePrefix = "TinyCollection";

        public static CollectionMapper Create(IDynamicAssembly assembly, MappingMember member)
        {
            TypePair typePair = member.TypePair;

            TypeBuilder typeBuilder = assembly.DefineType(GetMapperName(), typeof(CollectionMapper));
            if (IsList(typePair.Target))
            {
                EmitToList(typeBuilder, typePair);
            }

            var result = (CollectionMapper)Activator.CreateInstance(typeBuilder.CreateType());
            return result;
        }

        internal override TTarget MapCore<TSource, TTarget>(TSource source, TTarget target)
        {
            return EnumerableToList<TTarget>((IEnumerable)source);
        }

        protected virtual TTarget ConvertItem<TSource, TTarget>(TSource source)
        {
            throw new NotImplementedException();
        }

        protected virtual TTarget EnumerableToList<TTarget>(IEnumerable value)
        {
            throw new NotImplementedException();
        }

        protected List<TTarget> EnumerableToListTemplate<TSource, TTarget>(IEnumerable source)
        {
            var result = new List<TTarget>();
            foreach (TSource item in source)
            {
                TTarget convertItem = ConvertItem<TSource, TTarget>(item);
                result.Add(convertItem);
            }
            return result;
        }

        private static void EmitToList(TypeBuilder typeBuilder, TypePair typePair)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("EnumerableToList", OverrideProtected, Types.Object, new[] { Types.IEnumerable });

            Type sourceItemType = GetCollectionItemType(typePair.Source);
            Type targetItemType = GetCollectionItemType(typePair.Target);

            MethodInfo methodTemplate = ThisType()
                .GetMethod("EnumerableToListTemplate", InstanceNonPublic)
                .MakeGenericMethod(sourceItemType, targetItemType);

            IEmitterType returnValue = EmitterMethod.Call(methodTemplate, EmitterThis.Load(ThisType()), EmitterArgument.Load(Types.Object, 1));
            EmitterReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
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

        private static bool IsList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        private static Type ThisType()
        {
            return typeof(CollectionMapper);
        }
    }
}
