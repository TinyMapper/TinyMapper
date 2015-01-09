using System;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators;
using TinyMapper.CodeGenerators.Ast;
using TinyMapper.CodeGenerators.Ast.Statements;

namespace TinyMapper.Builders.Methods
{
    internal sealed class MapMembersMethodBuilder : EmitMethodBuilder
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
            astComposite.Add(LoadMethodArgument(localSource, 1))
                        .Add(LoadMethodArgument(localTarget, 2));

            astComposite.Add(new AstReturn(typeof(object), AstLoadLocal.Load(localTarget)));

            astComposite.Emit(codeGenerator);
        }

        protected override MethodBuilder CreateMethodBuilder(TypeBuilder typeBuilder)
        {
            return typeBuilder.DefineMethod(ObjectTypeMapper.MapMembersMethodName,
                MethodAttribute, typeof(object), new[] { typeof(object), typeof(object) });
        }

        /// <summary>
        /// Loads the method argument.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="argumentIndex">Index of the argument. 0 - This! (start from 1)</param>
        /// <returns><see cref="AstComposite"/></returns>
        private AstComposite LoadMethodArgument(LocalBuilder builder, int argumentIndex)
        {
            var result = new AstComposite();
            result.Add(new AstLocalVariableDeclaration(builder))
                  .Add(new AstStoreLocal(builder, AstLoadArgument.Load(typeof(object), argumentIndex)));
            return result;
        }
    }
}
