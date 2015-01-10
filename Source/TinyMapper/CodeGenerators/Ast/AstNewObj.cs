using System;
using System.Reflection;
using TinyMapper.Extensions;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class AstNewObj : IAstType
    {
        public AstNewObj(Type objectType)
        {
            ObjectType = objectType;
        }

        public Type ObjectType { get; private set; }

        public void Emit(CodeGenerator generator)
        {
            EmitRefType(generator);
        }

        private void EmitRefType(CodeGenerator generator)
        {
            ConstructorInfo ctor = ObjectType.GetDefaultCtor();

            generator.EmitNewObject(ctor);
        }
    }
}
