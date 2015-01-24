using System.Collections.Generic;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators.Emitters;
using TinyMapper.Core;
using TinyMapper.Mappers.Builders.Members;
using TinyMapper.Mappers.Types1;
using TinyMapper.Mappers.Types1.Members;

namespace TinyMapper.Mappers.Builders.Methods
{
    internal sealed class MapMembersMethodBuilder : EmitMethodBuilder
    {
        private readonly LocalBuilder _localSource;
        private readonly LocalBuilder _localTarget;

        public MapMembersMethodBuilder(MappingType mappingType, TypeBuilder typeBuilder)
            : base(mappingType, typeBuilder)
        {
            _localSource = _codeGenerator.DeclareLocal(mappingType.TypePair.Source);
            _localTarget = _codeGenerator.DeclareLocal(mappingType.TypePair.Target);
        }

        protected override void BuildCore()
        {
            var emitterComposite = new EmitterComposite();
            emitterComposite.Add(LoadMethodArgument(_localSource, 1))
                            .Add(LoadMethodArgument(_localTarget, 2));

            IEmitter node = EmitMappingMembers(_mappingType.Members);

            emitterComposite.Add(node);
            emitterComposite.Add(EmitterReturn.Return(EmitterLocal.Load(_localTarget)));
            emitterComposite.Emit(_codeGenerator);
        }

        protected override MethodBuilder CreateMethodBuilder(TypeBuilder typeBuilder)
        {
            return typeBuilder.DefineMethod(Mapper.MapMembersMethodName,
                MethodAttribute, Types.Object, new[] { Types.Object, Types.Object });
        }

        private IEmitter EmitMappingMembers(List<MappingMember> mappingMembers)
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
                  .Add(EmitterLocal.Store(builder, EmitterArgument.Load(Types.Object, argumentIndex)));
            return result;
        }
    }
}
