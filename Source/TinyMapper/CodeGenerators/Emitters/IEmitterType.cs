using System;

namespace TinyMapper.CodeGenerators.Emitters
{
    internal interface IEmitterType : IEmitter
    {
        Type ObjectType { get; }
    }
}
