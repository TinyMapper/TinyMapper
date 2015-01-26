using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators;
using TinyMapper.CodeGenerators.Emitters;
using TinyMapper.Core;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Types1.Members;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers.Collections
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

        internal override object MapCore(object source, object target)
        {
            return EnumerableToList((IEnumerable)source);
        }

        protected virtual object EnumerableToList(IEnumerable value)
        {
            throw new NotImplementedException();
        }

        protected List<TTarget> EnumerableToListTemplate<TTarget>(IEnumerable source)
        {
            var result = new List<TTarget>();
            foreach (object item in source)
            {
                result.Add((TTarget)item);
            }
            return result;
        }

        private static void EmitToList(TypeBuilder typeBuilder, TypePair typePair)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("EnumerableToList", OverrideProtected, Types.Object, new[] { Types.IEnumerable });

            Type targetItemType = GetCollectionItemType(typePair.Target);

            MethodInfo methodTemplate = ThisType()
                .GetMethod("EnumerableToListTemplate", InstanceNonPublic)
                .MakeGenericMethod(targetItemType);

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
