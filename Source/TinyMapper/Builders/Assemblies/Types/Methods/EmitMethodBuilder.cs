using System;
using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators;
using TinyMapper.Extensions;

namespace TinyMapper.Builders.Assemblies.Types.Methods
{
    internal abstract class EmitMethodBuilder
    {
        protected const MethodAttributes MethodAttribute = MethodAttributes.Assembly | MethodAttributes.Virtual;
        protected readonly CodeGenerator _codeGenerator;
        protected readonly Type _sourceType;
        protected readonly Type _targetType;

        protected EmitMethodBuilder(Type sourceType, Type targetType, TypeBuilder typeBuilder)
        {
            _sourceType = sourceType.IsNullable() ? Nullable.GetUnderlyingType(sourceType) : sourceType;
            _targetType = targetType.IsNullable() ? Nullable.GetUnderlyingType(targetType) : targetType;
            _codeGenerator = CreateCodeGenerator(typeBuilder);
        }

        public void Build()
        {
            BuildCore();
        }

        protected abstract void BuildCore();

        protected abstract MethodBuilder CreateMethodBuilder(TypeBuilder typeBuilder);

        private CodeGenerator CreateCodeGenerator(TypeBuilder typeBuilder)
        {
            MethodBuilder methodBuilder = CreateMethodBuilder(typeBuilder);
            return new CodeGenerator(methodBuilder.GetILGenerator());
        }
    }
}
