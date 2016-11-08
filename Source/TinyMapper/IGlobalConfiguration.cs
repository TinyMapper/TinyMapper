using System;

namespace Nelibur.ObjectMapper
{
    public interface IGlobalConfiguration
    {
        void ChangeNameMatching(Func<string, string, bool> nameMatching);
    }
}