using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper.Core.Extensions;

namespace Nelibur.ObjectMapper.CodeGenerators.Emitters
{
    internal sealed class EmitComposite : IEmitter
    {
        private readonly List<IEmitter> _nodes = new List<IEmitter>();

        public void Emit(CodeGenerator generator)
        {
            _nodes.ForEach(x => x.Emit(generator));
        }

        public EmitComposite Add(IEmitter node)
        {
            if (node.IsNotNull())
            {
                _nodes.Add(node);
            }
            return this;
        }
    }
}
