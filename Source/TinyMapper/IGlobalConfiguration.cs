using System;

namespace Nelibur.ObjectMapper
{
    public interface IGlobalConfiguration
    {
        void Reset();
        void ChangeNameMatching(Func<string, string, bool> nameMatching);
    }
}