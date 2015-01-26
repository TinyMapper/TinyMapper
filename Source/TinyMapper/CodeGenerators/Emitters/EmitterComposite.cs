using System;
using System.Collections.Generic;
using TinyMapper.Nelibur.Sword.Extensions;

namespace TinyMapper.CodeGenerators.Emitters
{
    internal sealed class EmitterComposite : IEmitter
    {
        private readonly List<IEmitter> _nodes = new List<IEmitter>();

        public EmitterComposite Add(IEmitter node)
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
