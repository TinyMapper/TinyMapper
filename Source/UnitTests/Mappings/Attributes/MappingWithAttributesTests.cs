using System;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Bindings;
using Xunit;

namespace UnitTests.Mappings.Attributes
{
    public sealed class MappingWithAttributesTests
    {
        [Fact]
        public void Map_WithAttributes_Success()
        {
            TinyMapper.Bind<SourceWithIgnore, TargetWithIgnore>();

            SourceWithIgnore source = CreateSourceWithIgnore();
            var actual = TinyMapper.Map<TargetWithIgnore>(source);

            Assert.Equal(default(DateTime), actual.DateTime);
            Assert.Equal(source.FirstName, actual.FirstName);
            Assert.Equal(source.LegacyString, actual.LatestString);
            Assert.Equal(default(string), actual.ProtectedString);
        }

        private static SourceWithIgnore CreateSourceWithIgnore()
        {
            return new SourceWithIgnore
            {
                FirstName = "John",
                DateTime = DateTime.Now,
                LegacyString = "LegacyString",
                SealedString = "SealedString"
            };
        }

        [Fact]
        public void Map_WithTargetSubset_Success()
        {
            TinyMapper.Bind<SourceWithSubset, TargetSubset1>();
            TinyMapper.Bind<SourceWithSubset, TargetSubset2>();

            SourceWithSubset source = CreateSourceWithSubset();
            var target1 = TinyMapper.Map<TargetSubset1>(source);

            Assert.Equal(default(DateTime), target1.DateTime);
            Assert.Equal(source.FirstName, target1.FirstName);
            Assert.Equal(source.SourceForTarget1and2, target1.LatestString);
            Assert.Equal(source.SourceForTarget1, target1.SourceString);
            Assert.NotEqual(source.SourceForTarget2, target1.SourceString);

            var target2 = TinyMapper.Map<TargetSubset2>(source);

            Assert.Equal(source.DateTime, target2.DateTime);
            Assert.Equal(source.FirstName, target2.FirstName);
            Assert.Equal(source.SourceForTarget1and2, target2.LatestString);
            Assert.NotEqual(source.SourceForTarget1, target2.SourceString);
            Assert.Equal(source.SourceForTarget2, target2.SourceString);
        }

        private static SourceWithSubset CreateSourceWithSubset()
        {
            return new SourceWithSubset
            {
                FirstName = "John",
                DateTime = DateTime.Now,
                SourceForTarget1and2 = "SourceForTarget1and2",
                SourceForTarget1 = "SourceForTarget1",
                SourceForTarget2 = "SourceForTarget2"
            };
        }

        public class SourceWithIgnore
        {
            [Ignore]
            public DateTime DateTime { get; set; }

            public string FirstName { get; set; }

            [Bind(nameof(TargetWithIgnore.LatestString))]
            public string LegacyString { get; set; }

            public string SealedString { get; set; }
        }


        public class TargetWithIgnore
        {
            public DateTime DateTime { get; set; }
            public string FirstName { get; set; }
            public string LatestString { get; set; }
            public string ProtectedString { get; set; }
        }


        public class SourceWithSubset
        {
            /// <summary>
            /// Ignore map for <see cref="TargetSubset1"/>, but not for <see cref="TargetSubset2"/>
            /// </summary>
            [Ignore(typeof(TargetSubset1))]
            public DateTime DateTime { get; set; }

            public string FirstName { get; set; }

            [Bind("LatestString")]
            public string SourceForTarget1and2 { get; set; }

            [Bind(nameof(TargetSubset1.SourceString), typeof(TargetSubset1))]
            public string SourceForTarget1 { get; set; }

            [Bind(nameof(TargetSubset2.SourceString), typeof(TargetSubset2))]
            public string SourceForTarget2 { get; set; }
        }


        public class TargetSubset1
        {
            public DateTime DateTime { get; set; }
            public string FirstName { get; set; }
            public string LatestString { get; set; }
            public string SourceString { get; set; }
        }


        public class TargetSubset2
        {
            public DateTime DateTime { get; set; }
            public string FirstName { get; set; }
            public string LatestString { get; set; }
            public string SourceString { get; set; }
        }
    }
}
