using System;
using System.Reflection.Emit;

namespace TinyMappers.CodeGenerators.Emitters
{
    internal sealed class EmitterReturn : IEmitterType
    {
        private readonly IEmitterType _returnValue;

        private EmitterReturn(IEmitterType returnValue, Type returnType)
        {
            ObjectType = returnType ?? returnValue.ObjectType;
            _returnValue = returnValue;
        }

        public Type ObjectType { get; private set; }

        public static IEmitterType Return(IEmitterType returnValue, Type returnType = null)
        {
            return new EmitterReturn(returnValue, returnType);
        }

        public void Emit(CodeGenerator generator)
        {
            _returnValue.Emit(generator);
            if (ObjectType == _returnValue.ObjectType)
            {
                generator.Emit(OpCodes.Ret);
            }
            else
            {
                generator.CastType(_returnValue.ObjectType, ObjectType)
                         .Emit(OpCodes.Ret);
            }
        }
    }
}
