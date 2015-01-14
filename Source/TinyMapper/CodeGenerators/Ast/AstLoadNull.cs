using System;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class AstLoadNull : IAstType
    {
        private AstLoadNull()
        {
            ObjectType = typeof(object);
        }

        public Type ObjectType { get; private set; }

        public static IAstType Load()
        {
            return new AstLoadNull();
        }

        public void Emit(CodeGenerator generator)
        {
            generator.Emit(OpCodes.Ldnull);
        }
    }
}
