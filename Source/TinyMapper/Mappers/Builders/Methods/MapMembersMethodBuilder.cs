using System.Collections.Generic;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators.Emitters;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Builders.Members;

namespace TinyMapper.Mappers.Builders.Methods
{
    internal sealed class MapMembersMethodBuilder : EmitMethodBuilder
    {
        private readonly LocalBuilder _localSource;
        private readonly LocalBuilder _localTarget;
        private readonly MemberSelector _memberSelector;

        public MapMembersMethodBuilder(TypePair typePair, TypeBuilder typeBuilder)
            : base(typePair, typeBuilder)
        {
            _memberSelector = new MemberSelector(typePair);
            _localSource = _codeGenerator.DeclareLocal(typePair.Source);
            _localTarget = _codeGenerator.DeclareLocal(typePair.Target);
        }

        protected override void BuildCore()
        {
            var astComposite = new EmitterComposite();
            astComposite.Add(LoadMethodArgument(_localSource, 1))
                        .Add(LoadMethodArgument(_localTarget, 2));

            List<SimpleMappingMember> mappingMembers = _memberSelector.GetMappingMembers();

            IEmitter node = EmitMappingMembers(mappingMembers);

            astComposite.Add(node);
            astComposite.Add(EmitterReturn.Return(EmitterLocal.Load(_localTarget)));
            astComposite.Emit(_codeGenerator);
        }

        protected override MethodBuilder CreateMethodBuilder(TypeBuilder typeBuilder)
        {
            return typeBuilder.DefineMethod(Mapper.MapMembersMethodName,
                MethodAttribute, typeof(object), new[] { typeof(object), typeof(object) });
        }

        private IEmitter EmitMappingMembers(List<SimpleMappingMember> mappingMembers)
        {
            MemberBuilder memberBuilder = MemberBuilder.Configure(x =>
            {
                x.LocalSource = _localSource;
                x.LocalTarget = _localTarget;
                x.CodeGenerator = _codeGenerator;
            }).Create();

            IEmitter result = memberBuilder.Build(mappingMembers);
            return result;
        }

        /// <summary>
        ///     Loads the method argument.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="argumentIndex">Index of the argument. 0 - This! (start from 1)</param>
        /// <returns>
        ///     <see cref="EmitterComposite" />
        /// </returns>
        private EmitterComposite LoadMethodArgument(LocalBuilder builder, int argumentIndex)
        {
            var result = new EmitterComposite();
            result.Add(EmitterLocalVariable.Declare(builder))
                  .Add(EmitterLocal.Store(builder, EmitterArgument.Load(typeof(object), argumentIndex)));
            return result;
        }
    }
}
