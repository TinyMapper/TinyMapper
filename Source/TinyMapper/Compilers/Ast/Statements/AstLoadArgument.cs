using System;
using System.Reflection.Emit;

namespace TinyMapper.Compilers.Ast.Statements
{
    internal abstract class AstLoadArgument : IAstType
    {
        private readonly int _index;

        private AstLoadArgument(Type type, int index)
        {
            ObjectType = type;
            _index = index;
        }

        public Type ObjectType { get; private set; }

        public static IAstType Load(Type type, int index)
        {
            var result = new AstLoadArgumentImpl(type, index);
            return result;
        }

        public abstract void Emit(CodeGenerator generator);


        private sealed class AstLoadArgumentImpl : AstLoadArgument
        {
            public AstLoadArgumentImpl(Type type, int index) : base(type, index)
            {
            }

            public override void Emit(CodeGenerator generator)
            {
                switch (_index)
                {
                    case 0:
                        generator.Emit(OpCodes.Ldarg_0);
                        break;
                    case 1:
                        generator.Emit(OpCodes.Ldarg_1);
                        break;
                    case 2:
                        generator.Emit(OpCodes.Ldarg_2);
                        break;
                    case 3:
                        generator.Emit(OpCodes.Ldarg_3);
                        break;
                    default:
                        generator.Emit(OpCodes.Ldarg, _index);
                        break;
                }
            }
        }
    }
}
