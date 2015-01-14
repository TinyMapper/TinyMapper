using System;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class AstStoreField : IAstType
    {
        private readonly FieldInfo _field;
        private readonly IAstType _targetObject;
        private readonly IAstType _value;

        private AstStoreField(FieldInfo field, IAstType targetObject, IAstType value)
        {
            _field = field;
            _targetObject = targetObject;
            _value = value;
            ObjectType = _field.FieldType;
        }

        public Type ObjectType { get; private set; }

        public static IAstNode Store(FieldInfo field, IAstType targetObject, IAstType value)
        {
            return new AstStoreField(field, targetObject, value);
        }

        public void Emit(CodeGenerator generator)
        {
            _targetObject.Emit(generator);
            _value.Emit(generator);
            generator.CastType(_value.ObjectType, _field.FieldType);
            generator.Emit(OpCodes.Stfld, _field);
        }
    }
}
