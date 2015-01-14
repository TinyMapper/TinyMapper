using System;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Ast
{
    internal static class EmitterArgument
    {
        public static IEmitterType Load(Type type, int index)
        {
            var result = new EmitterLoadArgument(type, index);
            return result;
        }


        private sealed class EmitterLoadArgument : IEmitterType
        {
            private readonly int _index;

            public EmitterLoadArgument(Type type, int index)
            {
                ObjectType = type;
                _index = index;
            }

            public Type ObjectType { get; private set; }

            public void Emit(CodeGenerator generator)
            {
                switch (_index)
                {
                    case 0:
                        generator.Emit(OpCodes.Ldarg_0);
                        break;
                    case 1:
                        generator.Emit(OpCodes.Ldarg_1);
                        break;
                    case 2:
                        generator.Emit(OpCodes.Ldarg_2);
                        break;
                    case 3:
                        generator.Emit(OpCodes.Ldarg_3);
                        break;
                    default:
                        generator.Emit(OpCodes.Ldarg, _index);
                        break;
                }
            }
        }
    }
}
