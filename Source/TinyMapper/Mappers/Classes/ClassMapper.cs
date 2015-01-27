using System;
using System.Collections.Generic;
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
    internal abstract class ClassMapper : Mapper
    {
        private const string MapperNamePrefix = "TinyClass";
        private static IDynamicAssembly _assembly;

        public static Mapper Create(IDynamicAssembly assembly, TypePair typePair)
        {
            _assembly = assembly;
            string mapperTypeName = GetMapperName(typePair);
            TypeBuilder typeBuilder = assembly.DefineType(mapperTypeName, typeof(ClassMapper));
            EmitCreateTargetInstance(typePair.Target, typeBuilder);
            Option<MapperCache> mappers = EmitMapClass(typePair, typeBuilder);

            var result = (ClassMapper)Activator.CreateInstance(typeBuilder.CreateType());
            mappers.Do(x => result.AddMappers(x.Mappers));
            return result;
        }

        internal override object MapCore(object source, object target)
        {
            if (target == null)
            {
                target = CreateTargetInstance();
            }
            object result = MapClass(source, target);
            return result;
        }

        protected virtual object CreateTargetInstance()
        {
            throw new NotImplementedException();
        }

        protected abstract object MapClass(object source, object target);

        private static void EmitCreateTargetInstance(Type targetType, TypeBuilder typeBuilder)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("CreateTargetInstance", OverrideProtected, Types.Object, Type.EmptyTypes);
            var codeGenerator = new CodeGenerator(methodBuilder.GetILGenerator());

            IEmitterType result = targetType.IsValueType ? EmitValueType(targetType, codeGenerator) : EmitRefType(targetType);

            EmitterReturn.Return(result, targetType).Emit(codeGenerator);
        }

        private static Option<MapperCache> EmitMapClass(TypePair typePair, TypeBuilder typeBuilder)
        {
            MappingType mappingType = MappingTypeBuilder.Build(typePair);

            MethodBuilder methodBuilder = typeBuilder.DefineMethod("MapClass", OverrideProtected, Types.Object,
                new[] { Types.Object, Types.Object });
            var codeGenerator = new CodeGenerator(methodBuilder.GetILGenerator());

            LocalBuilder localSource = codeGenerator.DeclareLocal(typePair.Source);
            LocalBuilder localTarget = codeGenerator.DeclareLocal(typePair.Target);

            var emitterComposite = new EmitterComposite();
            emitterComposite.Add(LoadMethodArgument(localSource, 1))
                            .Add(LoadMethodArgument(localTarget, 2));

            MemberEmitterDescription members = EmitMappingMembers(localSource, localTarget, mappingType.Members, codeGenerator);

            emitterComposite.Add(members.Emitter);
            emitterComposite.Add(EmitterReturn.Return(EmitterLocal.Load(localTarget)));
            emitterComposite.Emit(codeGenerator);
            return members.MapperCache;
        }

        private static MemberEmitterDescription EmitMappingMembers(LocalBuilder localSource, LocalBuilder localTarget,
            List<MappingMember> members, CodeGenerator codeGenerator)
        {
            MemberMapper memberMapper = MemberMapper.Configure(x =>
            {
                x.Assembly = _assembly;
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
