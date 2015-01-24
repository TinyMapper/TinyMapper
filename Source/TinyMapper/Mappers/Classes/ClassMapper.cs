using System;
using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators;
using TinyMapper.CodeGenerators.Emitters;
using TinyMapper.Core;
using TinyMapper.DataStructures;
using TinyMapper.Extensions;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers.Classes
{
    internal abstract class ClassMapper
    {
        private const string MapperNamePrefix = "TinyClass";
        private const MethodAttributes OverrideProtected = MethodAttributes.Family | MethodAttributes.Virtual;

        public static ClassMapper Create(IDynamicAssembly assembly, TypePair typePair)
        {
            string mapperTypeName = GetMapperName(typePair);
            TypeBuilder typeBuilder = assembly.DefineType(mapperTypeName, typeof(Mapper));
            EmitCreateTargetInstance(typePair.Target, typeBuilder);

            var result = (ClassMapper)Activator.CreateInstance(typeBuilder.CreateType());
            return result;
        }

        internal object Map(object source, object target)
        {
            if (target == null)
            {
                target = CreateTargetInstance();
            }

            throw new NotImplementedException();
            //            object result = MapCore(source, target);
            //            return result;
        }

        protected virtual object CreateTargetInstance()
        {
            throw new NotImplementedException();
        }

        private static void EmitCreateTargetInstance(Type targetType, TypeBuilder typeBuilder)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("CreateTargetInstance", OverrideProtected, Types.Object, Type.EmptyTypes);
            var codeGenerator = new CodeGenerator(methodBuilder.GetILGenerator());

            IEmitterType result = targetType.IsValueType ? EmitValueType(targetType, codeGenerator) : EmitRefType(targetType);

            EmitterReturn.Return(result, targetType).Emit(codeGenerator);
        }

        private static IEmitterType EmitRefType(Type type)
        {
            return type.HasDefaultCtor() ? EmitterNewObj.NewObj(type) : EmitterNull.Load();
        }

        private static IEmitterType EmitValueType(Type type, CodeGenerator codeGenerator)
        {
            LocalBuilder builder = codeGenerator.DeclareLocal(type);
            EmitterLocalVariable.Declare(builder).Emit(codeGenerator);
            return EmitterBox.Box(EmitterLocal.Load(builder));
        }

        private static string GetMapperName(TypePair pair)
        {
            string random = Guid.NewGuid().ToString("N");
            string sourceFullName = pair.Source.FullName;
            string targetFullName = pair.Target.FullName;
            return string.Format("{0}_{1}_{2}_{3}", MapperNamePrefix, sourceFullName, targetFullName, random);
        }
    }
}
