using System;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Emitters
{
    internal sealed class EmitterNull : IEmitterType
    {
        private EmitterNull()
        {
            ObjectType = typeof(object);
        }

        public Type ObjectType { get; private set; }

        public static IEmitterType Load()
        {
            return new EmitterNull();
        }

        public void Emit(CodeGenerator generator)
        {
            generator.Emit(OpCodes.Ldnull);
        }
    }
}
