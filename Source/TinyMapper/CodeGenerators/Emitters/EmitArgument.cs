using System;
using System.Reflection.Emit;

namespace Nelibur.ObjectMapper.CodeGenerators.Emitters
{
    internal static class EmitArgument
    {
        public static IEmitterType Load(Type type, int index)
        {
            var result = new EmitLoadArgument(type, index);
            return result;
        }


        private sealed class EmitLoadArgument : IEmitterType
        {
            private readonly int _index;

            public EmitLoadArgument(Type type, int index)
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
