using System;

namespace Nelibur.ObjectMapper.CodeGenerators.Emitters
{
    internal interface IEmitterType : IEmitter
    {
        Type ObjectType { get; }
    }
}
