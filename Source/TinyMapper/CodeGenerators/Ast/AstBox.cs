using System;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class AstBox : IAstType
    {
        private readonly IAstType _value;

        private AstBox(IAstType value)
        {
            _value = value;
            ObjectType = value.ObjectType;
        }

        public Type ObjectType { get; private set; }

        public static IAstType Box(IAstType value)
        {
            return new AstBox(value);
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
