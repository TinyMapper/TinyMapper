using System.Collections.Generic;
using TinyMapper.Nelibur.Sword.Extensions;

namespace TinyMapper.Compilers.Ast
{
    internal sealed class AstComposite : IAstNode
    {
        private readonly List<IAstNode> _nodes = new List<IAstNode>();

        public void Add(IAstNode node)
        {
            if (node.IsNull())
            {
                return;
            }
            _nodes.Add(node);
        }

        public void Emit(CodeGenerator generator)
        {
            _nodes.ForEach(x => x.Emit(generator));
        }
    }
}
