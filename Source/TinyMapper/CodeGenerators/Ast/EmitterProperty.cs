using System;
using System.Reflection;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class EmitterProperty
    {
        public static IEmitterType Load(IEmitterType source, PropertyInfo property)
        {
            return new EmitterLoadProperty(source, property);
        }

        public static IEmitterType Store(PropertyInfo property, IEmitterType targetObject, IEmitterType value)
        {
            return new EmitterStoreProperty(property, targetObject, value);
        }


        private class EmitterLoadProperty : IEmitterType
        {
            private readonly PropertyInfo _property;
            private readonly IEmitterType _source;

            public EmitterLoadProperty(IEmitterType source, PropertyInfo property)
            {
                _source = source;
                _property = property;
                ObjectType = property.PropertyType;
            }

            public Type ObjectType { get; private set; }

            public void Emit(CodeGenerator generator)
            {
                MethodInfo method = _property.GetGetMethod();
                EmitterMethod.Call(method, _source, null).Emit(generator);
            }
        }


        private sealed class EmitterStoreProperty : IEmitterType
        {
            private readonly IEmitterType _callMethod;

            public EmitterStoreProperty(PropertyInfo property, IEmitterType targetObject, IEmitterType value)
            {
                MethodInfo method = property.GetSetMethod();
                _callMethod = EmitterMethod.Call(method, targetObject, value);
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
