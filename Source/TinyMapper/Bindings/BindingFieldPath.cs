using System;
using System.Collections.Generic;

namespace Nelibur.ObjectMapper.Bindings
{
    internal sealed class BindingFieldPath
    {
        public BindingFieldPath(List<string> sourcePath, List<string> targetPath)
        {
            SourcePath = sourcePath;
            TargetPath = targetPath;
            HasPath = sourcePath.Count != 1 || targetPath.Count != 1;
            SourceHead = sourcePath[0];
            TargetHead = targetPath[0];
        }

        public List<string> SourcePath { get; }
        public List<string> TargetPath { get; }
        public string SourceHead { get; }
        public string TargetHead { get; }
        public bool HasPath { get; }
    }
}
