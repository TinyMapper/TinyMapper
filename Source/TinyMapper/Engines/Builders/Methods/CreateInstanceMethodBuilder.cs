using System;
using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.Compilers;
using TinyMapper.Compilers.Ast;
using TinyMapper.Compilers.Ast.Statements;
using TinyMapper.Extensions;
using TinyMapper.Mappers;
using TinyMapper.Nelibur.Sword.Extensions;

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

        private CodeGenerator CreateCodeGenerator(TypeBuilder typeBuilder)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(ObjectTypeMapper.CreateTargetInstanceMethodName,
                MethodAttributes.Assembly | MethodAttributes.Virtual, typeof(object), Type.EmptyTypes);

            return new CodeGenerator(methodBuilder.GetILGenerator());
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

            type.ToOption()
                .Map(_ => type.IsValueType, _ => CreateValueType(type, codeGenerator))
                .Map(_ => type.IsValueType == false, _ => CreateRefType(type))
                .Map(x => new AstReturn(type, x))
                .Do(x => x.Emit(codeGenerator));
        }
    }
}
