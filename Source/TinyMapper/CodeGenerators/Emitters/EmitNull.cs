using System;
using System.Reflection.Emit;

namespace Nelibur.ObjectMapper.CodeGenerators.Emitters
{
    internal sealed class EmitNull : IEmitterType
    {
        private EmitNull()
        {
            ObjectType = typeof(object);
        }

        public Type ObjectType { get; private set; }

        public static IEmitterType Load()
        {
            return new EmitNull();
        }

        public void Emit(CodeGenerator generator)
        {
            generator.Emit(OpCodes.Ldnull);
        }
    }
}
