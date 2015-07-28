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
            var actual = TinyMapper.Map<Target>(source);

            Assert.Equal(actual.DateTime, default(DateTime));
            Assert.Equal(actual.FirstName, source.FirstName);
            Assert.Equal(actual.LatestString, source.LegacyString);
            Assert.Equal(actual.ProtectedString, source.SealedString);
        }

        [Fact]
        public void Map_Back_Success()
        {
            TinyMapper.Bind<Target, Source>();

            Target target = CreateTarget();
            var actual = TinyMapper.Map<Source>(target);

            Assert.Equal(actual.DateTime, target.DateTime);
            Assert.Equal(actual.FirstName, target.FirstName);
            Assert.Equal(actual.LegacyString, target.LatestString);
            Assert.Equal(actual.SealedString, target.ProtectedString);
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

        private static Target CreateTarget()
        {
            return new Target
            {
                FirstName = "John",
                DateTime = DateTime.Now,
                LatestString = "legacy field",
                ProtectedString = "sealed field, we don't change legacy code",
            };
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
