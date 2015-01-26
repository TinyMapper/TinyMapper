using System;

namespace TinyMapper.CodeGenerators.Emitters
{
    internal static class EmitterThis
    {
        public static IEmitterType Load(Type thisType)
        {
            return EmitterArgument.Load(thisType, 0);
        }
    }
}
