using System;
using System.Reflection.Emit;

namespace Nelibur.ObjectMapper.CodeGenerators.Emitters
{
    internal static class EmitArray
    {
        public static IEmitterType Load(IEmitterType array, int index)
        {
            return new EmitLoadArray(array, index);
        }


        private sealed class EmitLoadArray : IEmitterType
        {
            private readonly IEmitterType _array;
            private readonly int _index;

            public EmitLoadArray(IEmitterType array, int index)
            {
                _array = array;
                _index = index;
                ObjectType = array.ObjectType.GetElementType();
            }

            public Type ObjectType { get; }

            public void Emit(CodeGenerator generator)
            {
                _array.Emit(generator);
                switch (_index)
                {
                    case 0:
                        generator.Emit(OpCodes.Ldc_I4_0);
                        break;
                    case 1:
                        generator.Emit(OpCodes.Ldc_I4_1);
                        break;
                    case 2:
                        generator.Emit(OpCodes.Ldc_I4_2);
                        break;
                    case 3:
                        generator.Emit(OpCodes.Ldc_I4_3);
                        break;
                    default:
                        generator.Emit(OpCodes.Ldc_I4, _index);
                        break;
                }
                generator.Emit(OpCodes.Ldelem, ObjectType);
            }
        }
    }
}
