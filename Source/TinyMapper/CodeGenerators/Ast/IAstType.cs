using System;

namespace TinyMapper.CodeGenerators.Ast
{
    internal interface IAstType : IAstNode
    {
        Type ObjectType { get; }
    }
}
