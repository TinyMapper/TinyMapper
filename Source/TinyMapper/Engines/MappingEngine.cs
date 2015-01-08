using System;
using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.Compilers;
using TinyMapper.Compilers.Ast;
using TinyMapper.Compilers.Ast.Statements;
using TinyMapper.Extensions;
using TinyMapper.Mappers;
using TinyMapper.Nelibur.Sword.Extensions;

namespace TinyMapper.Engines
{
    internal sealed class MappingEngine
    {
        public void CreateMapper(Type sourceType, Type targetType)
        {
            TypeBuilder typeBuilder = DynamicAssembly.DefineType("Test1", typeof(MarkerTypeMapper));
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
            return new AstBox(AstReadLocal.ReadLocal(builder));
        }

        private void EmitCreateTargetInstance(Type type, TypeBuilder typeBuilder)
        {
            if (type.IsNullable())
            {
                type = Nullable.GetUnderlyingType(type);
            }

            CodeGenerator codeGenerator = CreateCodeGenerator(typeBuilder);

            type.ToOption()
                .Map(_ => type.IsValueType, _ => CreateValueType(type, codeGenerator))
                .Map(_ => type.IsValueType == false, _ => CreateRefType(type))
                .Map(x => new AstReturn(type, x))
                .Do(x => x.Emit(codeGenerator));
        }
    }
}
