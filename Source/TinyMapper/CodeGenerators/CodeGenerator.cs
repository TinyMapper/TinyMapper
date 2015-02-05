using System;
using System.Reflection;
using System.Reflection.Emit;
using Nelibur.ObjectMapper.CodeGenerators.Emitters;
using Nelibur.ObjectMapper.Core.Extensions;

namespace Nelibur.ObjectMapper.CodeGenerators
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
            if (stackType.IsValueType == false && targetType == typeof(object))
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

        public CodeGenerator Emit(OpCode opCode, FieldInfo value)
        {
            _ilGenerator.Emit(opCode, value);
            return this;
        }

        public CodeGenerator EmitCall(MethodInfo method, IEmitterType invocationObject, params IEmitterType[] arguments)
        {
            ParameterInfo[] actualArguments = method.GetParameters();
            if (arguments.IsNull())
            {
                arguments = new IEmitterType[0];
            }
            if (arguments.Length != actualArguments.Length)
            {
                throw new ArgumentException();
            }

            if (invocationObject.IsNotNull())
            {
                invocationObject.Emit(this);
            }

            for (int i = 0; i < arguments.Length; i++)
            {
                arguments[i].Emit(this);
                CastType(arguments[i].ObjectType, actualArguments[i].ParameterType);
            }
            EmitCall(method);
            return this;
        }

        public CodeGenerator EmitNewObject(ConstructorInfo ctor)
        {
            _ilGenerator.Emit(OpCodes.Newobj, ctor);
            return this;
        }

        private void EmitCall(MethodInfo method)
        {
            if (method.IsVirtual)
            {
                _ilGenerator.EmitCall(OpCodes.Callvirt, method, Type.EmptyTypes);
            }
            else
            {
                _ilGenerator.EmitCall(OpCodes.Call, method, Type.EmptyTypes);
            }
        }
    }
}
