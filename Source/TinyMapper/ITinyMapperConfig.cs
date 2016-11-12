using System;

namespace Nelibur.ObjectMapper
{
    public interface ITinyMapperConfig
    {
        void NameMatching(Func<string, string, bool> nameMatching);
        void Reset();
    }
}
