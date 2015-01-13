using System;
using System.Reflection;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class AstLoadProperty : IAstType
    {
        private readonly PropertyInfo _property;
        private readonly IAstType _source;

        private AstLoadProperty(IAstType source, PropertyInfo property)
        {
            _source = source;
            _property = property;
            ObjectType = property.PropertyType;
        }

        public Type ObjectType { get; private set; }

        public static IAstType Load(IAstType source, PropertyInfo property)
        {
            return new AstLoadProperty(source, property);
        }

        public void Emit(CodeGenerator generator)
        {
            MethodInfo method = _property.GetGetMethod();
            AstCallMethod.Call(method, _source, null).Emit(generator);
        }
    }
}
