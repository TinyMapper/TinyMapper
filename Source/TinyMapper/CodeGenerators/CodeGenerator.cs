using System;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators
{
    internal sealed class CodeGenerator
    {
        private readonly ILGenerator _ilGenerator;

        public CodeGenerator(ILGenerator ilGenerator)
        {
            _ilGenerator = ilGenerator;
        }

        public CodeGenerator CastType(Type stackType, Type targetType)
        {
            if (stackType == targetType)
            {
                return this;
            }
            if (stackType.IsValueType && !targetType.IsValueType)
            {
                _ilGenerator.Emit(OpCodes.Box, stackType);
            }
            else if (!stackType.IsValueType && targetType.IsValueType)
            {
                _ilGenerator.Emit(OpCodes.Unbox_Any, targetType);
            }
            else
            {
                _ilGenerator.Emit(OpCodes.Castclass, targetType);
            }
            return this;
        }

        public LocalBuilder DeclareLocal(Type type)
        {
            return _ilGenerator.DeclareLocal(type);
        }

        public CodeGenerator Emit(OpCode opCode)
        {
            _ilGenerator.Emit(opCode);
            return this;
        }

        public CodeGenerator Emit(OpCode opCode, int value)
        {
            _ilGenerator.Emit(opCode, value);
            return this;
        }

        public CodeGenerator Emit(OpCode opCode, Type value)
        {
            _ilGenerator.Emit(opCode, value);
            return this;
        }

        public CodeGenerator EmitNewObject(ConstructorInfo ctor)
        {
            _ilGenerator.Emit(OpCodes.Newobj, ctor);
            return this;
        }
    }
}
