using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using TinyMappers.CodeGenerators;
using TinyMappers.CodeGenerators.Emitters;
using TinyMappers.Core;
using TinyMappers.DataStructures;
using TinyMappers.Extensions;
using TinyMappers.Mappers.Caches;
using TinyMappers.Mappers.Classes.Members;
using TinyMappers.Mappers.Types1;
using TinyMappers.Mappers.Types1.Members;
using TinyMappers.Nelibur.Sword.DataStructures;
using TinyMappers.Nelibur.Sword.Extensions;
using TinyMappers.Reflection;

namespace TinyMappers.Mappers.Classes
{
    internal class ClassMapperBuilder
    {
        private const string MapperNamePrefix = "TinyClass";
        private const MethodAttributes OverrideProtected = MethodAttributes.Family | MethodAttributes.Virtual;

        public static Mapper Create(IDynamicAssembly assembly, TypePair typePair)
        {
            string mapperTypeName = GetMapperName(typePair);
            Type parentType = typeof(ClassMapper<,>).MakeGenericType(typePair.Source, typePair.Target);
            TypeBuilder typeBuilder = assembly.DefineType(mapperTypeName, parentType);
            EmitCreateTargetInstance(typePair.Target, typeBuilder);
            Option<MapperCache> mappers = EmitMapClass(assembly, typePair, typeBuilder);

            var result = (Mapper)Activator.CreateInstance(typeBuilder.CreateType());
            mappers.Do(x => result.AddMappers(x.Mappers));
            return result;
        }

        private static void EmitCreateTargetInstance(Type targetType, TypeBuilder typeBuilder)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("CreateTargetInstance", OverrideProtected, targetType, Type.EmptyTypes);
            var codeGenerator = new CodeGenerator(methodBuilder.GetILGenerator());

            IEmitterType result = targetType.IsValueType ? EmitValueType(targetType, codeGenerator) : EmitRefType(targetType);

            EmitterReturn.Return(result, targetType).Emit(codeGenerator);
        }

        private static Option<MapperCache> EmitMapClass(IDynamicAssembly assembly, TypePair typePair, TypeBuilder typeBuilder)
        {
            MappingType mappingType = MappingTypeBuilder.Build(typePair);

            MethodBuilder methodBuilder = typeBuilder.DefineMethod("MapClass", OverrideProtected, typePair.Target,
                new[] { typePair.Source, typePair.Target});
            var codeGenerator = new CodeGenerator(methodBuilder.GetILGenerator());

            LocalBuilder localSource = codeGenerator.DeclareLocal(typePair.Source);
            LocalBuilder localTarget = codeGenerator.DeclareLocal(typePair.Target);

            var emitterComposite = new EmitterComposite();
//            emitterComposite.Add(LoadMethodArgument(localSource, 1))
//                            .Add(LoadMethodArgument(localTarget, 2));

            MemberEmitterDescription members = EmitMappingMembers(assembly, localSource, localTarget, mappingType.Members, codeGenerator);

            emitterComposite.Add(members.Emitter);
            emitterComposite.Add(EmitterReturn.Return(EmitterLocal.Load(localTarget)));
            emitterComposite.Emit(codeGenerator);
            return members.MapperCache;
        }

        private static MemberEmitterDescription EmitMappingMembers(IDynamicAssembly assembly, LocalBuilder localSource,
            LocalBuilder localTarget, List<MappingMember> members, CodeGenerator codeGenerator)
        {
            MemberMapper memberMapper = MemberMapper.Configure(x =>
            {
                x.Assembly = assembly;
                x.CodeGenerator = codeGenerator;
                x.LocalSource = localSource;
                x.LocalTarget = localTarget;
            }).Create();

            MemberEmitterDescription result = memberMapper.Build(members);
            return result;
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

        /// <summary>
        ///     Loads the method argument.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="argumentIndex">Index of the argument. 0 - This! (start from 1)</param>
        /// <returns>
        ///     <see cref="EmitterComposite" />
        /// </returns>
        private static EmitterComposite LoadMethodArgument(LocalBuilder builder, int argumentIndex)
        {
            var result = new EmitterComposite();
            result.Add(EmitterLocalVariable.Declare(builder))
                  .Add(EmitterLocal.Store(builder, EmitterArgument.Load(Types.Object, argumentIndex)));
            return result;
        }
    }
}
