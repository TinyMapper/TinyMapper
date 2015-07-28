using System;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Bindings;
using Xunit;

namespace UnitTests
{
    public sealed class MappingWithAttributesTests
    {
        [Fact]
        public void Map_Success()
        {
            // use custom attributes: 'IgnoreAttribute' and 'BindAttribute'
            TinyMapper.Bind<Source, Target>(config =>
            {
//                config.Ignore(x => x.DateTime);
//                config.Bind(from => from.LegacyString, to => to.LatestString);
//                config.Bind(from => from.SealedString, to => to.ProtectedString);
            });

            Source source = CreateSource();
            TinyMapper.Map<Target>(source);

            var actual = TinyMapper.Map<Target>(source);

            Assert.Equal(actual.DateTime, default(DateTime));
            Assert.Equal(actual.FirstName, source.FirstName);
            Assert.Equal(actual.LatestString, source.LegacyString);
            Assert.Equal(actual.ProtectedString, source.SealedString);
        }

        private static Source CreateSource()
        {
            return new Source
            {
                FirstName = "John",
                DateTime = DateTime.Now,
                LegacyString = "legacy field",
                SealedString = "sealed field, we don't change legacy code",
            };
        }

        private static Target HandmadeMap(Source source, Target target)
        {
//            target.DateTime = source.DateTime;
            target.FirstName = source.FirstName;
            target.LatestString = source.LegacyString;
            target.ProtectedString = source.SealedString;
            return target;
        }

        public class Source
        {
            [Ignore] // ignore set 'Target' field 'DateTime', by set 'Source' field 'DateTime'
            public DateTime DateTime { get; set; }
            public string FirstName { get; set; }
            [Bind("LatestString")] // set to 'Target' field 'LatestString'
            public string LegacyString { get; set; }
            public string SealedString { get; set; }
        }

        public class Target
        {
            public DateTime DateTime { get; set; }
            public string FirstName { get; set; }
            public string LatestString { get; set; }
            [Bind("SealedString")] // set from 'Source' field 'SealedString'
            public string ProtectedString { get; set; }
        }
    }
}
