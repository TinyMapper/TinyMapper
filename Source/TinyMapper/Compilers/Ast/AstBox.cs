using System;
using System.Reflection.Emit;

namespace TinyMapper.Compilers.Ast
{
    internal sealed class AstBox : IAstType
    {
        private readonly IAstType _value;

        public AstBox(IAstType value)
        {
            _value = value;
            ObjectType = value.ObjectType;
        }

        public Type ObjectType { get; private set; }

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
