namespace TinyMapper.CodeGenerators.Ast
{
    internal interface IAstNode
    {
        void Emit(CodeGenerator generator);
    }
}