using System;
using System.Reflection.Emit;
using TinyMapper.Compilers;
using TinyMapper.Compilers.Ast;
using TinyMapper.Compilers.Ast.Statements;
using TinyMapper.Extensions;
using TinyMapper.Mappers;

namespace TinyMapper.Engines.Builders.Methods
{
    internal sealed class CreateInstanceMethodBuilder : EmitMethodBuilder
    {
        public CreateInstanceMethodBuilder(Type sourceType, Type targetType, TypeBuilder typeBuilder)
            : base(sourceType, targetType, typeBuilder)
        {
        }

        protected override void BuildCore()
        {
            EmitMethod(_targetType, _typeBuilder);
        }

        protected override MethodBuilder CreateMethodBuilder(TypeBuilder typeBuilder)
        {
            return typeBuilder.DefineMethod(ObjectTypeMapper.CreateTargetInstanceMethodName,
                MethodAttribute, typeof(object), Type.EmptyTypes);
        }

        private IAstType CreateRefType(Type type)
        {
            if (type.HasDefaultCtor())
            {
                return new AstNewType(type);
            }
            return new AstNull();
        }

        private IAstType CreateValueType(Type type, CodeGenerator codeGenerator)
        {
            LocalBuilder builder = codeGenerator.DeclareLocal(type);
            new AstLocalVariableDeclaration(builder).Emit(codeGenerator);
            return new AstBox(AstLoadLocal.Load(builder));
        }

        private void EmitMethod(Type type, TypeBuilder typeBuilder)
        {
            CodeGenerator codeGenerator = CreateCodeGenerator(typeBuilder);

            IAstType value = type.IsValueType ? CreateValueType(type, codeGenerator) : CreateRefType(type);
            new AstReturn(type, value).Emit(codeGenerator);
        }
    }
}
