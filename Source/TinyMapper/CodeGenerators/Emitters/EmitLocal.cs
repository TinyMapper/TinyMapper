using System;
using System.Reflection.Emit;

namespace Nelibur.ObjectMapper.CodeGenerators.Emitters
{
    internal static class EmitLocal
    {
        public static IEmitterType Load(LocalBuilder localBuilder)
        {
            var result = new EmitLoadLocal(localBuilder);
            return result;
        }


        private sealed class EmitLoadLocal : IEmitterType
        {
            private readonly LocalBuilder _localBuilder;

            public EmitLoadLocal(LocalBuilder localBuilder)
            {
                _localBuilder = localBuilder;
                ObjectType = localBuilder.LocalType;
            }

            public Type ObjectType { get; }

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
    }
}
