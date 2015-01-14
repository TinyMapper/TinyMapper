using System;
using System.Reflection;

namespace TinyMapper.CodeGenerators.Ast
{
    internal sealed class AstStoreProperty : IAstType
    {
        private readonly IAstType _callMethod;

        private AstStoreProperty(PropertyInfo property, IAstType targetObject, IAstType value)
        {
            MethodInfo method = property.GetSetMethod();
            _callMethod = AstCallMethod.Call(method, targetObject, value);
            ObjectType = _callMethod.ObjectType;
        }

        public Type ObjectType { get; private set; }

        public static IAstNode Store(PropertyInfo property, IAstType targetObject, IAstType value)
        {
            return new AstStoreProperty(property, targetObject, value);
        }

        public void Emit(CodeGenerator generator)
        {
            _callMethod.Emit(generator);
        }
    }
}
