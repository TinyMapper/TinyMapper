//using System.Reflection;
//using System.Reflection.Emit;
//using TinyMapper.CodeGenerators;
//using TinyMapper.Mappers.Types1;
//
//namespace TinyMapper.Mappers.Builders.Methods
//{
//    internal abstract class EmitMethodBuilder
//    {
//        protected const MethodAttributes MethodAttribute = MethodAttributes.Assembly | MethodAttributes.Virtual;
//        protected readonly CodeGenerator _codeGenerator;
//        protected readonly MappingType _mappingType;
//
//        protected EmitMethodBuilder(MappingType mappingType, TypeBuilder typeBuilder)
//        {
//            _mappingType = mappingType;
//            _codeGenerator = CreateCodeGenerator(typeBuilder);
//        }
//
//        public void Build()
//        {
//            BuildCore();
//        }
//
//        protected abstract void BuildCore();
//
//        protected abstract MethodBuilder CreateMethodBuilder(TypeBuilder typeBuilder);
//
//        private CodeGenerator CreateCodeGenerator(TypeBuilder typeBuilder)
//        {
//            MethodBuilder methodBuilder = CreateMethodBuilder(typeBuilder);
//            return new CodeGenerator(methodBuilder.GetILGenerator());
//        }
//    }
//}
