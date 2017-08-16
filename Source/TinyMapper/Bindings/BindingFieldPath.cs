using System;
using System.Collections.Generic;

namespace Nelibur.ObjectMapper.Bindings
{
    internal sealed class BindingFieldPath
    {
        public BindingFieldPath(List<string> path)
        {
            Path = path;
            Head = path[0];
            Tail = path[path.Count - 1];
            HasPath = path.Count > 1;
        }

        public List<string> Path { get; }
        public string Head { get; }
        public string Tail { get; }
        public bool HasPath { get; }
    }
}
