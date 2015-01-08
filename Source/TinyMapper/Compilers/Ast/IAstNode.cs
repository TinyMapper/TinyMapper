namespace TinyMapper.Compilers.Ast
{
    internal interface IAstNode
    {
        void Emit(CodeGenerator generator);
    }
}