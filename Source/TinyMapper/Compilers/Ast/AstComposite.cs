using System.Collections.Generic;
using TinyMapper.Nelibur.Sword.Extensions;

namespace TinyMapper.Compilers.Ast
{
    internal sealed class AstComposite : IAstNode
    {
        private readonly List<IAstNode> _nodes = new List<IAstNode>();

        public AstComposite Add(IAstNode node)
        {
            if (node.IsNotNull())
            {
                _nodes.Add(node);
            }
            return this;
        }

        public void Emit(CodeGenerator generator)
        {
            _nodes.ForEach(x => x.Emit(generator));
        }
    }
}
