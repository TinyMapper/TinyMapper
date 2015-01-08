using System;

namespace TinyMapper.Compilers.Ast
{
    internal interface IAstType : IAstNode
    {
        Type ObjectType { get; }
    }
}
