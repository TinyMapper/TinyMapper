using System;
using System.Reflection.Emit;
using TinyMappers.Nelibur.Sword.DataStructures;
using TinyMappers.Nelibur.Sword.Extensions;

namespace TinyMappers.CodeGenerators.Emitters
{
    internal sealed class EmitterLocalVariable : IEmitterType
    {
        private readonly Option<LocalBuilder> _localBuilder;

        private EmitterLocalVariable(LocalBuilder localBuilder)
        {
            _localBuilder = localBuilder.ToOption();
            ObjectType = localBuilder.LocalType;
        }

        public Type ObjectType { get; private set; }

        public static IEmitterType Declare(LocalBuilder localBuilder)
        {
            return new EmitterLocalVariable(localBuilder);
        }

        public void Emit(CodeGenerator generator)
        {
            _localBuilder.Where(x => x.LocalType.IsValueType)
                         .Do(x => generator.Emit(OpCodes.Ldloca, x.LocalIndex))
                         .Do(x => generator.Emit(OpCodes.Initobj, x.LocalType));
        }
    }
}
