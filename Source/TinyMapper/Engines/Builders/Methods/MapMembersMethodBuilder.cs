using System;
using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.Compilers;
using TinyMapper.Compilers.Ast;
using TinyMapper.Compilers.Ast.Statements;
using TinyMapper.Mappers;

namespace TinyMapper.Engines.Builders.Methods
{
    internal sealed class MapMembersMethodBuilder : TargetMethodBuilder
    {
        public MapMembersMethodBuilder(Type sourceType, Type targetType, TypeBuilder typeBuilder)
            : base(sourceType, targetType, typeBuilder)
        {
        }

        protected override void BuildCore()
        {
            CodeGenerator codeGenerator = CreateCodeGenerator(_typeBuilder);
            LocalBuilder localSource = codeGenerator.DeclareLocal(_sourceType);
            LocalBuilder localTarget = codeGenerator.DeclareLocal(_targetType);

            var astComposite = new AstComposite();
            astComposite.Add(new AstLocalVariableDeclaration(localSource))
                        .Add(new AstLocalVariableDeclaration(localTarget));

            astComposite.Add(new AstReturn(typeof(object), AstLoadLocal.Load(localTarget)));

            astComposite.Emit(codeGenerator);
        }

        private CodeGenerator CreateCodeGenerator(TypeBuilder typeBuilder)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(ObjectTypeMapper.MapMembersMethodName,
                MethodAttributes.Assembly | MethodAttributes.Virtual, typeof(object), new Type[] { typeof(object), typeof(object) });

            return new CodeGenerator(methodBuilder.GetILGenerator());
        }
    }
}
