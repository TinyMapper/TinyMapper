using System;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class AstNull : IAstType
    {
        public AstNull()
        {
            ObjectType = typeof(object);
        }

        public Type ObjectType { get; private set; }

        public void Emit(CodeGenerator generator)
        {
            generator.Emit(OpCodes.Ldnull);
        }
    }
}
