using System;
using System.Reflection;
using TinyMapper.Extensions;

namespace TinyMapper.Compilers.Ast
{
    internal sealed class AstNewType : IAstType
    {
        public AstNewType(Type objectType)
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
