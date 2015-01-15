using System;
using System.Reflection.Emit;
using TinyMapper.Nelibur.Sword.DataStructures;
using TinyMapper.Nelibur.Sword.Extensions;

namespace TinyMapper.CodeGenerators.Emitters
{
    internal sealed class EmitterLocalVariableDeclaration : IEmitterType
    {
        private readonly Option<LocalBuilder> _localBuilder;

        private EmitterLocalVariableDeclaration(LocalBuilder localBuilder)
        {
            _localBuilder = localBuilder.ToOption();
            ObjectType = localBuilder.LocalType;
        }

        public Type ObjectType { get; private set; }

        public static IEmitterType Declare(LocalBuilder localBuilder)
        {
            return new EmitterLocalVariableDeclaration(localBuilder);
        }

        public void Emit(CodeGenerator generator)
        {
            _localBuilder.Where(x => x.LocalType.IsValueType)
                         .Do(x => generator.Emit(OpCodes.Ldloca, x.LocalIndex))
                         .Do(x => generator.Emit(OpCodes.Initobj, x.LocalType));
        }
    }
}
