using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nelibur.ObjectMapper.Mappers;

namespace Nelibur.ObjectMapper
{
    internal class GlobalConfiguration : IGlobalConfiguration
    {
        private readonly TargetMapperBuilder _targetMapperBuilder;

        public GlobalConfiguration(TargetMapperBuilder targetMapperBuilder)
        {
            if (targetMapperBuilder == null)
                throw new ArgumentNullException(nameof(targetMapperBuilder));

            _targetMapperBuilder = targetMapperBuilder;
        }

        public void ChangeNameMatching(Func<string, string, bool> nameMatching)
        {
            if (nameMatching == null)
                throw new ArgumentNullException(nameof(nameMatching));

            _targetMapperBuilder.IsNameMatched = nameMatching;
        }
    }
}
