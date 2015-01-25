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
        private const string MapperNamePrefix = "TinyCollection";
        private const MethodAttributes OverrideProtected = MethodAttributes.Family | MethodAttributes.Virtual;

        public static CollectionMapper Create(IDynamicAssembly assembly, MappingMember member)
        {
            TypePair typePair = member.TypePair;

            TypeBuilder typeBuilder = assembly.DefineType(GetMapperName(), typeof(CollectionMapper));
            if (IsList(typePair.Target))
            {
                MethodBuilder methodBuilder = typeBuilder.DefineMethod("ConvertToList", OverrideProtected, Types.Object,
                    new[] { typeof(IEnumerable) });

                Type targetItemType = GetCollectionItemType(typePair.Target);
                MethodInfo methodTemplate = ThisType()
                    .GetMethod("ConvertToListTemplate", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(targetItemType);

                IEmitterType returnValue = EmitterMethod.Call(methodTemplate, EmitterThis.Load(ThisType()), EmitterArgument.Load(Types.Object, 1));
                EmitterReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
            }

            var result = (CollectionMapper)Activator.CreateInstance(typeBuilder.CreateType());
            return result;
        }

        internal override object MapCore(object source, object target)
        {
            return ConvertToList((IEnumerable)source);
        }

        protected virtual object ConvertToList(IEnumerable value)
        {
            throw new NotImplementedException();
        }

        protected List<TTarget> ConvertToListTemplate<TTarget>(IEnumerable source)
        {
            var result = new List<TTarget>();
            foreach (object item in source)
            {
                result.Add((TTarget)item);
            }
            return result;
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
