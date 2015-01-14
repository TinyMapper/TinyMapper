using System;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Ast
{
    internal abstract class AstLoadLocal : IAstType
    {
        private readonly LocalBuilder _localBuilder;

        private AstLoadLocal(LocalBuilder localBuilder)
        {
            _localBuilder = localBuilder;
        }

        public Type ObjectType { get; private set; }

        public static IAstType Load(LocalBuilder localBuilder)
        {
            var result = new AstLoadLocalImpl(localBuilder);
            return result;
        }

        public static IAstType LoadAddress(LocalBuilder localBuilder)
        {
            if (localBuilder.LocalType.IsValueType)
            {
                return new AstLoadLocalAddressImpl(localBuilder);
            }
            return new AstLoadLocalImpl(localBuilder);
        }

        public abstract void Emit(CodeGenerator generator);


        private sealed class AstLoadLocalAddressImpl : AstLoadLocal
        {
            public AstLoadLocalAddressImpl(LocalBuilder localBuilder)
                : base(localBuilder)
            {
                ObjectType = localBuilder.LocalType.MakeByRefType();
            }

            public override void Emit(CodeGenerator generator)
            {
                generator.Emit(OpCodes.Ldloca, _localBuilder.LocalIndex);
            }
        }


        private sealed class AstLoadLocalImpl : AstLoadLocal
        {
            public AstLoadLocalImpl(LocalBuilder localBuilder) : base(localBuilder)
            {
                ObjectType = localBuilder.LocalType;
            }

            public override void Emit(CodeGenerator generator)
            {
                generator.Emit(OpCodes.Ldloc, _localBuilder.LocalIndex);
            }
        }
    }
}
