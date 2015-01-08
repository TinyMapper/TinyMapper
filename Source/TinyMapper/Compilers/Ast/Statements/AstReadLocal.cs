using System;
using System.Reflection.Emit;

namespace TinyMapper.Compilers.Ast.Statements
{
    internal abstract class AstReadLocal : IAstType
    {
        private readonly LocalBuilder _localBuilder;

        protected AstReadLocal(LocalBuilder localBuilder)
        {
            _localBuilder = localBuilder;
        }

        public Type ObjectType { get; private set; }

        public static IAstType ReadLocal(LocalBuilder localBuilder)
        {
            if (localBuilder.LocalType.IsValueType)
            {
                return new AstReadLocalValue(localBuilder);
            }
            return new AstReadLocalRef(localBuilder);
        }

        public abstract void Emit(CodeGenerator generator);


        private sealed class AstReadLocalRef : AstReadLocal
        {
            public AstReadLocalRef(LocalBuilder localBuilder) : base(localBuilder)
            {
            }

            public override void Emit(CodeGenerator generator)
            {
                generator.Emit(OpCodes.Ldloc, _localBuilder.LocalIndex);
            }
        }


        private sealed class AstReadLocalValue : AstReadLocal
        {
            public AstReadLocalValue(LocalBuilder localBuilder) : base(localBuilder)
            {
            }

            public override void Emit(CodeGenerator generator)
            {
                generator.Emit(OpCodes.Ldloc, _localBuilder.LocalIndex);
            }
        }
    }
}
