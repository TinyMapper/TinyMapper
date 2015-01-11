using System;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class AstReturn : IAstType
    {
        private readonly IAstType _returnValue;

        private AstReturn(Type returnType, IAstType returnValue)
        {
            _returnValue = returnValue;
            ObjectType = returnType;
        }

        public Type ObjectType { get; private set; }

        public static IAstType Return(Type returnType, IAstType returnValue)
        {
            return new AstReturn(returnType, returnValue);
        }

        public void Emit(CodeGenerator generator)
        {
            _returnValue.Emit(generator);
            generator.CastType(_returnValue.ObjectType, ObjectType)
                     .Emit(OpCodes.Ret);
        }
    }
}
