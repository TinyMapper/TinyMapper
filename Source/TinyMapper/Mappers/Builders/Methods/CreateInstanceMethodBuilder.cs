//using System;
//using System.Reflection.Emit;
//using TinyMapper.CodeGenerators;
//using TinyMapper.CodeGenerators.Emitters;
//using TinyMapper.Core;
//using TinyMapper.Extensions;
//using TinyMapper.Mappers.Types1;
//
//namespace TinyMapper.Mappers.Builders.Methods
//{
//    internal sealed class CreateInstanceMethodBuilder : EmitMethodBuilder
//    {
//        public CreateInstanceMethodBuilder(MappingType mappingType, TypeBuilder typeBuilder)
//            : base(mappingType, typeBuilder)
//        {
//        }
//
//        protected override void BuildCore()
//        {
//            EmitMethod(_mappingType.TypePair.Target);
//        }
//
//        protected override MethodBuilder CreateMethodBuilder(TypeBuilder typeBuilder)
//        {
//            return typeBuilder.DefineMethod(Mapper.CreateTargetInstanceMethodName,
//                MethodAttribute, Types.Object, Type.EmptyTypes);
//        }
//
//        private IEmitterType CreateRefType(Type type)
//        {
//            return type.HasDefaultCtor() ? EmitterNewObj.NewObj(type) : EmitterNull.Load();
//        }
//
//        private IEmitterType CreateValueType(Type type, CodeGenerator codeGenerator)
//        {
//            LocalBuilder builder = codeGenerator.DeclareLocal(type);
//            EmitterLocalVariable.Declare(builder).Emit(codeGenerator);
//            return EmitterBox.Box(EmitterLocal.Load(builder));
//        }
//
//        private void EmitMethod(Type type)
//        {
//            IEmitterType value = type.IsValueType ? CreateValueType(type, _codeGenerator) : CreateRefType(type);
//            EmitterReturn.Return(value, type).Emit(_codeGenerator);
//        }
//    }
//}
