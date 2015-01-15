using System;
using System.Reflection;

namespace TinyMapper.CodeGenerators.Emitters
{
    internal static class EmitterMethod
    {
        public static IEmitterType Call(MethodInfo method, IEmitterType invocationObject, params IEmitterType[] arguments)
        {
            return new EmitterCallMethod(method, invocationObject, arguments);
        }


        private sealed class EmitterCallMethod : IEmitterType
        {
            private readonly IEmitterType[] _arguments;
            private readonly IEmitterType _invocationObject;
            private readonly MethodInfo _method;

            public EmitterCallMethod(MethodInfo method, IEmitterType invocationObject, params IEmitterType[] arguments)
            {
                _method = method;
                _invocationObject = invocationObject;
                _arguments = arguments;
                ObjectType = _method.ReturnType;
            }

            public Type ObjectType { get; private set; }

            public void Emit(CodeGenerator generator)
            {
                generator.EmitCall(_method, _invocationObject, _arguments);
            }
        }
    }
}
