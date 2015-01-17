using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators;
using TinyMapper.DataStructures;

namespace TinyMapper.Builders.Assemblies.Types.Methods
{
    internal abstract class EmitMethodBuilder
    {
        protected const MethodAttributes MethodAttribute = MethodAttributes.Assembly | MethodAttributes.Virtual;
        protected readonly CodeGenerator _codeGenerator;
        protected readonly TypePair _typePair;

        protected EmitMethodBuilder(TypePair typePair, TypeBuilder typeBuilder)
        {
            _typePair = typePair;
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
