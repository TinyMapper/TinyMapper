using System;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Emitters
{
    internal static class EmitterField
    {
        public static IEmitterType Load(IEmitterType source, FieldInfo field)
        {
            var result = new EmitterLoadField(source, field);
            return result;
        }

        public static IEmitterType Store(FieldInfo field, IEmitterType targetObject, IEmitterType value)
        {
            return new EmitterStoreField(field, targetObject, value);
        }


        private sealed class EmitterLoadField : IEmitterType
        {
            private readonly FieldInfo _field;
            private readonly IEmitterType _source;

            public EmitterLoadField(IEmitterType source, FieldInfo field)
            {
                _source = source;
                _field = field;
                ObjectType = field.FieldType;
            }

            public Type ObjectType { get; private set; }

            public void Emit(CodeGenerator generator)
            {
                _source.Emit(generator);
                generator.Emit(OpCodes.Ldfld, _field);
            }
        }


        private sealed class EmitterStoreField : IEmitterType
        {
            private readonly FieldInfo _field;
            private readonly IEmitterType _targetObject;
            private readonly IEmitterType _value;

            public EmitterStoreField(FieldInfo field, IEmitterType targetObject, IEmitterType value)
            {
                _field = field;
                _targetObject = targetObject;
                _value = value;
                ObjectType = _field.FieldType;
            }

            public Type ObjectType { get; private set; }

            public void Emit(CodeGenerator generator)
            {
                _targetObject.Emit(generator);
                _value.Emit(generator);
                generator.CastType(_value.ObjectType, _field.FieldType);
                generator.Emit(OpCodes.Stfld, _field);
            }
        }
    }
}
