using System;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Emitters
{
    internal static class EmitterLocal
    {
        public static IEmitterType Load(LocalBuilder localBuilder)
        {
            var result = new EmitterLoadLocal(localBuilder);
            return result;
        }

        public static IEmitterType LoadAddress(LocalBuilder localBuilder)
        {
            if (localBuilder.LocalType.IsValueType)
            {
                return new EmitterLoadLocalAddress(localBuilder);
            }
            return new EmitterLoadLocal(localBuilder);
        }

        public static IEmitterType Store(LocalBuilder localBuilder, IEmitterType value)
        {
            return new EmitterStoreLocal(localBuilder, value);
        }


        private sealed class EmitterLoadLocal : IEmitterType
        {
            private readonly LocalBuilder _localBuilder;

            public EmitterLoadLocal(LocalBuilder localBuilder)
            {
                _localBuilder = localBuilder;
                ObjectType = localBuilder.LocalType;
            }

            public Type ObjectType { get; private set; }

            public void Emit(CodeGenerator generator)
            {
                switch (_localBuilder.LocalIndex)
                {
                    case 0:
                        generator.Emit(OpCodes.Ldloc_0);
                        break;
                    case 1:
                        generator.Emit(OpCodes.Ldloc_1);
                        break;
                    case 2:
                        generator.Emit(OpCodes.Ldloc_2);
                        break;
                    case 3:
                        generator.Emit(OpCodes.Ldloc_3);
                        break;
                    default:
                        generator.Emit(OpCodes.Ldloc, _localBuilder.LocalIndex);
                        break;
                }
            }
        }


        private sealed class EmitterLoadLocalAddress : IEmitterType
        {
            private readonly LocalBuilder _localBuilder;

            public EmitterLoadLocalAddress(LocalBuilder localBuilder)
            {
                _localBuilder = localBuilder;
                ObjectType = localBuilder.LocalType.MakeByRefType();
            }

            public Type ObjectType { get; private set; }

            public void Emit(CodeGenerator generator)
            {
                generator.Emit(OpCodes.Ldloca, _localBuilder.LocalIndex);
            }
        }


        private sealed class EmitterStoreLocal : IEmitterType
        {
            private readonly LocalBuilder _localBuilder;
            private readonly IEmitterType _value;

            public EmitterStoreLocal(LocalBuilder localBuilder, IEmitterType value)
            {
                _localBuilder = localBuilder;
                _value = value;
                ObjectType = _localBuilder.LocalType;
            }

            public Type ObjectType { get; private set; }

            public void Emit(CodeGenerator generator)
            {
                _value.Emit(generator);
                generator.CastType(_value.ObjectType, _localBuilder.LocalType);
                switch (_localBuilder.LocalIndex)
                {
                    case 0:
                        generator.Emit(OpCodes.Stloc_0);
                        break;
                    case 1:
                        generator.Emit(OpCodes.Stloc_1);
                        break;
                    case 2:
                        generator.Emit(OpCodes.Stloc_2);
                        break;
                    case 3:
                        generator.Emit(OpCodes.Stloc_3);
                        break;
                    default:
                        generator.Emit(OpCodes.Stloc, _localBuilder.LocalIndex);
                        break;
                }
            }
        }
    }
}
