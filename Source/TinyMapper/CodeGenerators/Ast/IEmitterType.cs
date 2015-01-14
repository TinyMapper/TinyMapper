using System;

namespace TinyMapper.CodeGenerators.Ast
{
    internal interface IEmitterType : IEmitter
    {
        Type ObjectType { get; }
    }
}
