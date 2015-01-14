using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TinyMapper.Builders.Assemblies.Types.Members;
using TinyMapper.CodeGenerators.Ast;

namespace TinyMapper.Builders.Assemblies.Types.Methods
{
    internal sealed class MapMembersMethodBuilder : EmitMethodBuilder
    {
        private readonly LocalBuilder _localSource;
        private readonly LocalBuilder _localTarget;
        private readonly MemberSelector _memberSelector;

        public MapMembersMethodBuilder(Type sourceType, Type targetType, TypeBuilder typeBuilder)
            : base(sourceType, targetType, typeBuilder)
        {
            _memberSelector = new MemberSelector(sourceType, targetType);
            _localSource = _codeGenerator.DeclareLocal(_sourceType);
            _localTarget = _codeGenerator.DeclareLocal(_targetType);
        }

        protected override void BuildCore()
        {
            var astComposite = new AstComposite();
            astComposite.Add(LoadMethodArgument(_localSource, 1))
                        .Add(LoadMethodArgument(_localTarget, 2));

            List<MappingMember> mappingMembers = _memberSelector.GetMappingMembers();

            IAstNode node = EmitMappingMembers(mappingMembers);

            astComposite.Add(node);
            astComposite.Add(AstReturn.Return(typeof(object), AstLoadLocal.Load(_localTarget)));
            astComposite.Emit(_codeGenerator);
        }

        protected override MethodBuilder CreateMethodBuilder(TypeBuilder typeBuilder)
        {
            return typeBuilder.DefineMethod(ObjectTypeBuilder.MapMembersMethodName,
                MethodAttribute, typeof(object), new[] { typeof(object), typeof(object) });
        }

        private IAstNode EmitMappingMembers(List<MappingMember> mappingMembers)
        {
            MemberBuilder memberBuilder = MemberBuilder.Configure(x =>
            {
                x.LocalSource = _localSource;
                x.LocalTarget = _localTarget;
                x.CodeGenerator = _codeGenerator;
            }).Create();

            IAstNode result = memberBuilder.Build(mappingMembers);
            return result;
        }

        /// <summary>
        ///     Loads the method argument.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="argumentIndex">Index of the argument. 0 - This! (start from 1)</param>
        /// <returns>
        ///     <see cref="AstComposite" />
        /// </returns>
        private AstComposite LoadMethodArgument(LocalBuilder builder, int argumentIndex)
        {
            var result = new AstComposite();
            result.Add(AstLocalVariableDeclaration.Declare(builder))
                  .Add(AstStoreLocal.Store(builder, AstLoadArgument.Load(typeof(object), argumentIndex)));
            return result;
        }
    }
}
