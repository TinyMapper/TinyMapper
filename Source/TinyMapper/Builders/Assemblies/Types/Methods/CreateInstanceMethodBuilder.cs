using System;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators;
using TinyMapper.CodeGenerators.Ast;
using TinyMapper.Extensions;

namespace TinyMapper.Builders.Assemblies.Types.Methods
{
    internal sealed class CreateInstanceMethodBuilder : EmitMethodBuilder
    {
        public CreateInstanceMethodBuilder(Type sourceType, Type targetType, TypeBuilder typeBuilder)
            : base(sourceType, targetType, typeBuilder)
        {
        }

        protected override void BuildCore()
        {
            EmitMethod(_targetType);
        }

        protected override MethodBuilder CreateMethodBuilder(TypeBuilder typeBuilder)
        {
            return typeBuilder.DefineMethod(ObjectTypeBuilder.CreateTargetInstanceMethodName,
                MethodAttribute, typeof(object), Type.EmptyTypes);
        }

        private IAstType CreateRefType(Type type)
        {
            if (type.HasDefaultCtor())
            {
                return new AstNewObj(type);
            }
            return new AstLoadNull();
        }

        private IAstType CreateValueType(Type type, CodeGenerator codeGenerator)
        {
            LocalBuilder builder = codeGenerator.DeclareLocal(type);
            new AstLocalVariableDeclaration(builder).Emit(codeGenerator);
            return new AstBox(AstLoadLocal.Load(builder));
        }

        private void EmitMethod(Type type)
        {
            IAstType value = type.IsValueType ? CreateValueType(type, _codeGenerator) : CreateRefType(type);
            new AstReturn(type, value).Emit(_codeGenerator);
        }
    }
}
