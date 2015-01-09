using System;
using System.Reflection.Emit;

namespace TinyMapper.CodeGenerators.Ast.Statements
{
    internal abstract class AstLoadLocal : IAstType
    {
        private readonly LocalBuilder _localBuilder;

        private AstLoadLocal(LocalBuilder localBuilder)
        {
            _localBuilder = localBuilder;
            ObjectType = localBuilder.LocalType;
        }

        public Type ObjectType { get; private set; }

        public static IAstType Load(LocalBuilder localBuilder)
        {
            var result = new AstLoadLocalImpl(localBuilder);
            return result;
        }

        public abstract void Emit(CodeGenerator generator);


        private sealed class AstLoadLocalImpl : AstLoadLocal
        {
            public AstLoadLocalImpl(LocalBuilder localBuilder) : base(localBuilder)
            {
            }

            public override void Emit(CodeGenerator generator)
            {
                generator.Emit(OpCodes.Ldloc, _localBuilder.LocalIndex);
            }
        }
    }
}
