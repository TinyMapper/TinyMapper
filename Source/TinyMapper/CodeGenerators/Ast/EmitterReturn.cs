using System;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class EmitterReturn : IEmitterType
    {
        private readonly IEmitterType _returnValue;

        private EmitterReturn(Type returnType, IEmitterType returnValue)
        {
            _returnValue = returnValue;
            ObjectType = returnType;
        }

        public Type ObjectType { get; private set; }

        public static IEmitterType Return(Type returnType, IEmitterType returnValue)
        {
            return new EmitterReturn(returnType, returnValue);
        }

        public void Emit(CodeGenerator generator)
        {
            _returnValue.Emit(generator);
            generator.CastType(_returnValue.ObjectType, ObjectType)
                     .Emit(OpCodes.Ret);
        }
    }
}
