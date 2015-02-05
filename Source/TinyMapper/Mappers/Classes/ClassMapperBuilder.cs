using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Nelibur.ObjectMapper.CodeGenerators;
using Nelibur.ObjectMapper.CodeGenerators.Emitters;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;
using Nelibur.ObjectMapper.Mappers.Caches;
using Nelibur.ObjectMapper.Mappers.Classes.Members;
using Nelibur.ObjectMapper.Mappers.Types1;
using Nelibur.ObjectMapper.Mappers.Types1.Members;
using Nelibur.ObjectMapper.Reflection;

namespace Nelibur.ObjectMapper.Mappers.Classes
{
    internal class ClassMapperBuilder
    {
        private const string CreateTargetInstanceMethod = "CreateTargetInstance";
        private const string MapClassMethod = "MapClass";
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
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(CreateTargetInstanceMethod, OverrideProtected, targetType, Type.EmptyTypes);
            var codeGenerator = new CodeGenerator(methodBuilder.GetILGenerator());

            IEmitterType result = targetType.IsValueType ? EmitValueType(targetType, codeGenerator) : EmitRefType(targetType);

            EmitReturn.Return(result, targetType).Emit(codeGenerator);
        }

        private static Option<MapperCache> EmitMapClass(IDynamicAssembly assembly, TypePair typePair, TypeBuilder typeBuilder)
        {
            MappingType mappingType = MappingTypeBuilder.Build(typePair);

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(MapClassMethod, OverrideProtected, typePair.Target,
                new[] { typePair.Source, typePair.Target });
            var codeGenerator = new CodeGenerator(methodBuilder.GetILGenerator());

            var emitterComposite = new EmitComposite();

            MemberEmitterDescription members = EmitMappingMembers(assembly, mappingType.Members, codeGenerator);

            emitterComposite.Add(members.Emitter);
            emitterComposite.Add(EmitReturn.Return(EmitArgument.Load(typePair.Target, 2)));
            emitterComposite.Emit(codeGenerator);
            return members.MapperCache;
        }

        private static MemberEmitterDescription EmitMappingMembers(IDynamicAssembly assembly, List<MappingMember> members, CodeGenerator codeGenerator)
        {
            MemberMapper memberMapper = MemberMapper.Configure(x => { x.Assembly = assembly; }).Create();

            MemberEmitterDescription result = memberMapper.Build(members);
            return result;
        }

        private static IEmitterType EmitRefType(Type type)
        {
            return type.HasDefaultCtor() ? EmitNewObj.NewObj(type) : EmitNull.Load();
        }

        private static IEmitterType EmitValueType(Type type, CodeGenerator codeGenerator)
        {
            LocalBuilder builder = codeGenerator.DeclareLocal(type);
            EmitLocalVariable.Declare(builder).Emit(codeGenerator);
            return EmitBox.Box(EmitLocal.Load(builder));
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
