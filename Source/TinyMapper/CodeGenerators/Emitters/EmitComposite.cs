using System.Collections.Generic;
using TinyMappers.Nelibur.Sword.Extensions;

namespace TinyMappers.CodeGenerators.Emitters
{
    internal sealed class EmitComposite : IEmitter
    {
        private readonly List<IEmitter> _nodes = new List<IEmitter>();

        public EmitComposite Add(IEmitter node)
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
