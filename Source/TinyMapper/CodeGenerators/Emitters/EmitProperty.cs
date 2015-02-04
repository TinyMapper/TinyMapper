using System;
using System.Reflection;

namespace Nelibur.Mapper.CodeGenerators.Emitters
{
    internal sealed class EmitProperty
    {
        public static IEmitterType Load(IEmitterType source, PropertyInfo property)
        {
            return new EmitLoadProperty(source, property);
        }

        public static IEmitterType Store(PropertyInfo property, IEmitterType targetObject, IEmitterType value)
        {
            return new EmitStoreProperty(property, targetObject, value);
        }


        private class EmitLoadProperty : IEmitterType
        {
            private readonly PropertyInfo _property;
            private readonly IEmitterType _source;

            public EmitLoadProperty(IEmitterType source, PropertyInfo property)
            {
                _source = source;
                _property = property;
                ObjectType = property.PropertyType;
            }

            public Type ObjectType { get; private set; }

            public void Emit(CodeGenerator generator)
            {
                MethodInfo method = _property.GetGetMethod();
                EmitMethod.Call(method, _source, null).Emit(generator);
            }
        }


        private sealed class EmitStoreProperty : IEmitterType
        {
            private readonly IEmitterType _callMethod;

            public EmitStoreProperty(PropertyInfo property, IEmitterType targetObject, IEmitterType value)
            {
                MethodInfo method = property.GetSetMethod();
                _callMethod = EmitMethod.Call(method, targetObject, value);
                ObjectType = _callMethod.ObjectType;
            }

            public Type ObjectType { get; private set; }

            public void Emit(CodeGenerator generator)
            {
                _callMethod.Emit(generator);
            }
        }
    }
}
