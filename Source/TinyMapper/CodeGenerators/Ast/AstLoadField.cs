using System;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Ast
{
    internal abstract class AstLoadField : IAstType
    {
        private readonly FieldInfo _field;
        private readonly IAstType _source;

        private AstLoadField(IAstType source, FieldInfo field)
        {
            _source = source;
            _field = field;
            ObjectType = field.FieldType;
        }

        public Type ObjectType { get; private set; }

        public static IAstType Load(IAstType source, FieldInfo field)
        {
            var result = new AstLoadFieldImpl(source, field);
            return result;
        }

        public abstract void Emit(CodeGenerator generator);


        private sealed class AstLoadFieldImpl : AstLoadField
        {
            public AstLoadFieldImpl(IAstType source, FieldInfo field) : base(source, field)
            {
            }

            public override void Emit(CodeGenerator generator)
            {
                _source.Emit(generator);
                generator.Emit(OpCodes.Ldfld, _field);
            }
        }
    }
}
