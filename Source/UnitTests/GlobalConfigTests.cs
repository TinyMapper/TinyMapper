using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests.Mappings
{
    public class GlobalConfigTests
    {
        [Fact(DisplayName = "Checks if the Name-Mating is used.")]
        public void CustomeMatching_Success()
        {
            TinyMapper.Config.ChangeNameMatching((s, t) => string.Equals(s + "1", t, StringComparison.OrdinalIgnoreCase));

            var source = new SourceCustom() { Name= "Hallo" };
            var result = new TargetCustom();
            TinyMapper.Map(source, result );

            Assert.Equal(source.Name, result.Name1);

        }

        public class SourceCustom
        {
            public string Name { get; set; }
        }


        public class TargetCustom
        {
            public string Name1 { get; set; }
        }

    }
}
