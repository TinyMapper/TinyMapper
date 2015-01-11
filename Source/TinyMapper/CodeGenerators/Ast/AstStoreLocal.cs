using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class AstStoreLocal : IAstNode
    {
        private readonly LocalBuilder _localBuilder;
        private readonly IAstType _value;

        private AstStoreLocal(LocalBuilder localBuilder, IAstType value)
        {
            _localBuilder = localBuilder;
            _value = value;
        }

        public static IAstNode Store(LocalBuilder localBuilder, IAstType value)
        {
            return new AstStoreLocal(localBuilder, value);
        }

        public void Emit(CodeGenerator generator)
        {
            _value.Emit(generator);
            generator.CastType(_value.ObjectType, _localBuilder.LocalType);
            generator.Emit(OpCodes.Stloc, _localBuilder.LocalIndex);
        }
    }
}
