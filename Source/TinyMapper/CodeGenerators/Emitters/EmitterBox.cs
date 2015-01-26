using System;
using System.Reflection.Emit;

namespace TinyMappers.CodeGenerators.Emitters
{
    internal sealed class EmitterBox : IEmitterType
    {
        private readonly IEmitterType _value;

        private EmitterBox(IEmitterType value)
        {
            _value = value;
            ObjectType = value.ObjectType;
        }

        public Type ObjectType { get; private set; }

        public static IEmitterType Box(IEmitterType value)
        {
            return new EmitterBox(value);
        }

        public void Emit(CodeGenerator generator)
        {
            _value.Emit(generator);

            if (ObjectType.IsValueType)
            {
                generator.Emit(OpCodes.Box, ObjectType);
            }
        }
    }
}
