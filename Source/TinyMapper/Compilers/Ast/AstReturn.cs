using System;
using System.Reflection.Emit;

namespace TinyMapper.Compilers.Ast
{
    internal sealed class AstReturn : IAstType
    {
        private readonly IAstType _returnValue;

        public AstReturn(Type returnType, IAstType returnValue)
        {
            _returnValue = returnValue;
            ObjectType = returnType;
        }

        public Type ObjectType { get; private set; }

        public void Emit(CodeGenerator generator)
        {
            _returnValue.Emit(generator);
            generator.CastType(_returnValue.ObjectType, ObjectType)
                     .Emit(OpCodes.Ret);
        }
    }
}
