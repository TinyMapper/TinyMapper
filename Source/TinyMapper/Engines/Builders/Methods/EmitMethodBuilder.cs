using System;
using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.Compilers;
using TinyMapper.Extensions;

namespace TinyMapper.Engines.Builders.Methods
{
    internal abstract class EmitMethodBuilder
    {
        protected const MethodAttributes MethodAttribute = MethodAttributes.Assembly | MethodAttributes.Virtual;
        protected readonly Type _sourceType;
        protected readonly Type _targetType;
        protected readonly TypeBuilder _typeBuilder;

        protected EmitMethodBuilder(Type sourceType, Type targetType, TypeBuilder typeBuilder)
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

        protected CodeGenerator CreateCodeGenerator(TypeBuilder typeBuilder)
        {
            MethodBuilder methodBuilder = CreateMethodBuilder(typeBuilder);
            return new CodeGenerator(methodBuilder.GetILGenerator());
        }

        protected abstract MethodBuilder CreateMethodBuilder(TypeBuilder typeBuilder);
    }
}
