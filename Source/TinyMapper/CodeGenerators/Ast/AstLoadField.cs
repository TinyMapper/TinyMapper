using System;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Ast
{
    internal abstract class AstLoadField : IAstType
    {
        private readonly FieldInfo _field;
        private readonly IAstType _value;

        protected AstLoadField(FieldInfo field, IAstType value)
        {
            _field = field;
            _value = value;
            ObjectType = field.FieldType;
        }

        public Type ObjectType { get; private set; }

        public static IAstType Load(FieldInfo field, IAstType value)
        {
            var result = new AstLoadFieldImpl(field, value);
            return result;
        }

        public abstract void Emit(CodeGenerator generator);


        private sealed class AstLoadFieldImpl : AstLoadField
        {
            public AstLoadFieldImpl(FieldInfo field, IAstType value) : base(field, value)
            {
            }

            public override void Emit(CodeGenerator generator)
            {
                _value.Emit(generator);
                generator.Emit(OpCodes.Ldfld, _field);
            }
        }
    }
}
