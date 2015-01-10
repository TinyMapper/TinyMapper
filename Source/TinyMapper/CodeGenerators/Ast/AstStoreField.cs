using System.Reflection;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class AstStoreField : IAstNode
    {
        private readonly FieldInfo _field;
        private readonly IAstType _targetValue;
        private readonly IAstType _value;

        public AstStoreField(FieldInfo field, IAstType targetValue, IAstType value)
        {
            _field = field;
            _targetValue = targetValue;
            _value = value;
        }

        public void Emit(CodeGenerator generator)
        {
            _targetValue.Emit(generator);
            _value.Emit(generator);
            generator.CastType(_value.ObjectType, _field.FieldType);
            generator.Emit(OpCodes.Stfld, _field);
        }
    }
}
