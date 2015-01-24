using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators;
using TinyMapper.CodeGenerators.Emitters;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Types.Members;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers.Collection
{
    internal abstract class CollectionMapper
    {
        private const string MapperNamePrefix = "TinyCollection";
        private const MethodAttributes OverrideProtected = MethodAttributes.Family | MethodAttributes.Virtual;

        public static CollectionMapper Create(IDynamicAssembly dynamicAssembly, MappingMember member)
        {
            TypePair typePair = member.TypePair;

            TypeBuilder typeBuilder = dynamicAssembly.DefineType(GetMapperName(), typeof(CollectionMapper));
            if (IsList(typePair.Target))
            {
                MethodBuilder methodBuilder = typeBuilder.DefineMethod("ConvertToList", OverrideProtected, typeof(object),
                    new[] { typeof(IEnumerable) });

                Type targetItemType = GetCollectionItemType(typePair.Target);
                MethodInfo methodTemplate = ThisType().GetMethod("ConvertToListTemplate", BindingFlags.NonPublic)
                                                      .MakeGenericMethod(targetItemType);

                IEmitterType returnValue = EmitterMethod.Call(methodTemplate, EmitterThis.Load(ThisType()), EmitterArgument.Load(typeof(object), 1));
                EmitterReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
            }

            var result = (CollectionMapper)Activator.CreateInstance(typeBuilder.CreateType());
            return result;
        }

        internal object Map(object source, object target)
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
