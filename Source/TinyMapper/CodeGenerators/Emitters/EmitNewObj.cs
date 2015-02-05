using System;
using System.Reflection;
using Nelibur.ObjectMapper.Core.Extensions;

namespace Nelibur.ObjectMapper.CodeGenerators.Emitters
{
    internal sealed class EmitNewObj : IEmitterType
    {
        private EmitNewObj(Type objectType)
        {
            ObjectType = objectType;
        }

        public Type ObjectType { get; private set; }

        public static IEmitterType NewObj(Type objectType)
        {
            return new EmitNewObj(objectType);
        }

        public void Emit(CodeGenerator generator)
        {
            ConstructorInfo ctor = ObjectType.GetDefaultCtor();
            generator.EmitNewObject(ctor);
        }
    }
}
