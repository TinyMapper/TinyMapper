using System;
using System.Reflection;
using TinyMapper.Extensions;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class AstNewObj : IAstType
    {
        private AstNewObj(Type objectType)
        {
            ObjectType = objectType;
        }

        public Type ObjectType { get; private set; }

        public static IAstType NewObj(Type objectType)
        {
            return new AstNewObj(objectType);
        }

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
