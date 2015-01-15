using System;
using System.Reflection;
using TinyMapper.Extensions;

namespace TinyMapper.CodeGenerators.Emitters
{
    internal sealed class EmitterNewObj : IEmitterType
    {
        private EmitterNewObj(Type objectType)
        {
            ObjectType = objectType;
        }

        public Type ObjectType { get; private set; }

        public static IEmitterType NewObj(Type objectType)
        {
            return new EmitterNewObj(objectType);
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
