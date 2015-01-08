using System;
using System.Reflection.Emit;
using TinyMapper.Extensions;

namespace TinyMapper.Engines.Builders.Methods
{
    internal abstract class TargetMethodBuilder
    {
        protected readonly Type _sourceType;
        protected readonly Type _targetType;
        protected readonly TypeBuilder _typeBuilder;

        protected TargetMethodBuilder(Type sourceType, Type targetType, TypeBuilder typeBuilder)
        {
            _sourceType = sourceType.IsNullable() ? Nullable.GetUnderlyingType(sourceType) : sourceType;
            _targetType = targetType.IsNullable() ? Nullable.GetUnderlyingType(targetType) : targetType;
            _typeBuilder = typeBuilder;
        }

        public void Build()
        {
            BuildCore();
        }

        protected abstract void BuildCore();
    }
}
