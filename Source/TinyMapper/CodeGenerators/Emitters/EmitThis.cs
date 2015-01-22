using System;

namespace TinyMapper.CodeGenerators.Emitters
{
    internal static class EmitThis
    {
        public static IEmitterType Load(Type thisType)
        {
            return EmitterArgument.Load(thisType, 0);
        }
    }
}