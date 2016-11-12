using System;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
{
    public class TinyMapperConfigTests
    {
        [Fact(DisplayName = "Checks if the Name-Matching is used.")]
        public void CustomeNameMatching_Success()
        {
            TinyMapper.Config(config =>
            {
                config.NameMatching((x, y) => string.Equals(x + "1", y, StringComparison.OrdinalIgnoreCase));
            });

            var sourceCustom = new SourceCustom { Custom = "Hello", Default = "Default" };
            var targetCustom = TinyMapper.Map<TargetCustom>(sourceCustom);

            Assert.Equal(sourceCustom.Custom, targetCustom.Custom1);
            Assert.True(string.IsNullOrEmpty(targetCustom.Default));

            TinyMapper.Config(config =>
            {
                config.Reset();
            });

            var sourceDefault = new SourceDefault { Custom = "Hello", Default = "Default" };
            var targetDefault = TinyMapper.Map<TargetDefault>(sourceDefault);

            Assert.True(string.IsNullOrEmpty(targetDefault.Custom1));
            Assert.Equal(sourceDefault.Default, targetDefault.Default);
        }

        public class SourceCustom
        {
            public string Custom { get; set; }
            public string Default { get; set; }
        }


        public class TargetCustom
        {
            public string Custom1 { get; set; }
            public string Default { get; set; }
        }

        public class SourceDefault
        {
            public string Custom { get; set; }
            public string Default { get; set; }
        }


        public class TargetDefault
        {
            public string Custom1 { get; set; }
            public string Default { get; set; }
        }

    }
}
