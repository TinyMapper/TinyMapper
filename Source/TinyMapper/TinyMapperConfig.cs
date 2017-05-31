using System;
using Nelibur.ObjectMapper.Mappers;

namespace Nelibur.ObjectMapper
{
    internal sealed class TinyMapperConfig : ITinyMapperConfig
    {
        private readonly TargetMapperBuilder _targetMapperBuilder;

        public bool EnablePolymorphicMapping { get; set; } = true;
        public bool EnableAutoBinding { get; set; } = true;

        public TinyMapperConfig(TargetMapperBuilder targetMapperBuilder)
        {
            if (targetMapperBuilder == null)
            {
                throw new ArgumentNullException();
            }
            _targetMapperBuilder = targetMapperBuilder;
        }

        public void NameMatching(Func<string, string, bool> nameMatching)
        {
            if (nameMatching == null)
            {
                throw new ArgumentNullException();
            }
            _targetMapperBuilder.SetNameMatching(nameMatching);
        }

        public void Reset()
        {
            _targetMapperBuilder.SetNameMatching(TargetMapperBuilder.DefaultNameMatching);
            EnableAutoBinding = true;
            EnablePolymorphicMapping = true;
        }
    }
}
