using System;

namespace Nelibur.ObjectMapper.CodeGenerators.Emitters
{
    internal interface IEmitter
    {
        void Emit(CodeGenerator generator);
    }
}
