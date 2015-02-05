using System;
using System.Reflection.Emit;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;

namespace Nelibur.ObjectMapper.CodeGenerators.Emitters
{
    internal sealed class EmitLocalVariable : IEmitterType
    {
        private readonly Option<LocalBuilder> _localBuilder;

        private EmitLocalVariable(LocalBuilder localBuilder)
        {
            _localBuilder = localBuilder.ToOption();
            ObjectType = localBuilder.LocalType;
        }

        public Type ObjectType { get; private set; }

        public static IEmitterType Declare(LocalBuilder localBuilder)
        {
            return new EmitLocalVariable(localBuilder);
        }

        public void Emit(CodeGenerator generator)
        {
            _localBuilder.Where(x => x.LocalType.IsValueType)
                         .Do(x => generator.Emit(OpCodes.Ldloca, x.LocalIndex))
                         .Do(x => generator.Emit(OpCodes.Initobj, x.LocalType));
        }
    }
}
