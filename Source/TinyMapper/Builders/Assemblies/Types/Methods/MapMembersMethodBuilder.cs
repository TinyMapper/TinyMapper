using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TinyMapper.Builders.Assemblies.Types.Members;
using TinyMapper.CodeGenerators;
using TinyMapper.CodeGenerators.Ast;

namespace TinyMapper.Builders.Assemblies.Types.Methods
{
    internal sealed class MapMembersMethodBuilder : EmitMethodBuilder
    {
        private readonly MemberSelector _memberSelector;

        public MapMembersMethodBuilder(Type sourceType, Type targetType, TypeBuilder typeBuilder)
            : base(sourceType, targetType, typeBuilder)
        {
            _memberSelector = new MemberSelector(sourceType, targetType);
        }

        protected override void BuildCore()
        {
            CodeGenerator codeGenerator = CreateCodeGenerator(_typeBuilder);
            LocalBuilder localSource = codeGenerator.DeclareLocal(_sourceType);
            LocalBuilder localTarget = codeGenerator.DeclareLocal(_targetType);

            var astComposite = new AstComposite();
            astComposite.Add(LoadMethodArgument(localSource, 1))
                        .Add(LoadMethodArgument(localTarget, 2));

            List<MappingMember> mappingMembers = _memberSelector.GetMappingMembers();

            astComposite.Add(new AstReturn(typeof(object), AstLoadLocal.Load(localTarget)));

            astComposite.Emit(codeGenerator);
        }

        protected override MethodBuilder CreateMethodBuilder(TypeBuilder typeBuilder)
        {
            return typeBuilder.DefineMethod(ObjectTypeBuilder.MapMembersMethodName,
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
