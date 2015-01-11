using System;
using System.Reflection;

namespace TinyMapper.CodeGenerators.Ast
{
    internal abstract class AstCallMethod : IAstType
    {
        private readonly IAstType[] _arguments;
        private readonly IAstType _invocationObject;
        private readonly MethodInfo _method;

        private AstCallMethod(MethodInfo method, IAstType invocationObject, params IAstType[] arguments)
        {
            _method = method;
            _invocationObject = invocationObject;
            _arguments = arguments;
            ObjectType = _method.ReturnType;
        }

        public Type ObjectType { get; private set; }

        public static IAstType Call(MethodInfo method, IAstType invocationObject, params IAstType[] arguments)
        {
            return new AstCallMethodImpl(method, invocationObject, arguments);
        }

        public abstract void Emit(CodeGenerator generator);


        private sealed class AstCallMethodImpl : AstCallMethod
        {
            public AstCallMethodImpl(MethodInfo method, IAstType invocationObject, params IAstType[] arguments) : base(method, invocationObject, arguments)
            {
            }

            public override void Emit(CodeGenerator generator)
            {
                generator.EmitCall(_method, _invocationObject, _arguments);
            }
        }
    }
}
