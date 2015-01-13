using System.Reflection;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class AstStoreProperty : IAstNode
    {
        private readonly PropertyInfo _property;
        private readonly IAstType _targetObject;
        private readonly IAstType _value;

        private AstStoreProperty(PropertyInfo property, IAstType targetObject, IAstType value)
        {
            _property = property;
            _targetObject = targetObject;
            _value = value;
        }

        public static IAstNode Store(PropertyInfo property, IAstType targetObject, IAstType value)
        {
            return new AstStoreProperty(property, targetObject, value);
        }

        public void Emit(CodeGenerator generator)
        {
            MethodInfo method = _property.GetSetMethod();
            AstCallMethod.Call(method, _targetObject, _value).Emit(generator);
        }
    }
}
