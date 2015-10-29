using System;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Bindings;
using Xunit;

namespace UnitTests.Mappings.Attributes
{
    public sealed class MappingWithAttributesTests
    {
        [Fact]
        public void Map_Success()
        {
            // use custom attributes: 'IgnoreAttribute' and 'BindAttribute'
            TinyMapper.Bind<Source, Target>(config =>
            {
                config.Ignore(x => x.DateTime);
                config.Bind(from => from.LegacyString, to => to.LatestString);
                config.Bind(from => from.SealedString, to => to.ProtectedString);
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

        [Fact]
        public void Map_WithType_Success()
        {
            TinyMapper.Bind<Source1, Target1>();
            TinyMapper.Bind<Source1, Target2>();

            Source1 source = CreateSource1();
            var target1 = TinyMapper.Map<Target1>(source);

            Assert.Equal(target1.DateTime, default(DateTime));
            Assert.Equal(target1.FirstName, source.FirstName);

            Assert.Equal(target1.LatestString, source.LegacyString);
            Assert.Equal(target1.SourceString, source.LegacyTarget1String);
            Assert.NotEqual(target1.SourceString, source.LegacyTarget2String);

            Assert.Equal(target1.ProtectedString, source.SealedString);
            Assert.Equal(target1.TargetString, source.SealedTarget1String);
            Assert.NotEqual(target1.TargetString, source.SealedTarget2String);

            var target2 = TinyMapper.Map<Target2>(source);

            Assert.Equal(target2.DateTime, source.DateTime);
            Assert.Equal(target2.FirstName, source.FirstName);

            Assert.Equal(target2.LatestString, source.LegacyString);
            Assert.NotEqual(target2.SourceString, source.LegacyTarget1String);
            Assert.Equal(target2.SourceString, source.LegacyTarget2String);

            Assert.Equal(target2.ProtectedString, source.SealedString);
            Assert.NotEqual(target2.TargetString, source.SealedTarget1String);
            Assert.Equal(target2.TargetString, source.SealedTarget2String);
        }

        [Fact]
        public void Map_WithType_Back_Success()
        {
            TinyMapper.Bind<Target1, Source1>();
            TinyMapper.Bind<Target2, Source1>();

            Target1 target1 = CreateTarget1();
            var actual = TinyMapper.Map<Source1>(target1);

            Assert.Equal(actual.DateTime, target1.DateTime);
            Assert.Equal(actual.FirstName, target1.FirstName);
            Assert.Equal(actual.LegacyString, target1.LatestString);
            Assert.Equal(actual.LegacyTarget1String, target1.SourceString);
            Assert.Null(actual.LegacyTarget2String);
            Assert.Equal(actual.SealedString, target1.ProtectedString);
            Assert.Equal(actual.SealedTarget1String, target1.TargetString);
            Assert.Null(actual.SealedTarget2String);

            Target2 target2 = CreateTarget2();
            actual = TinyMapper.Map<Source1>(target2);

            Assert.Equal(actual.DateTime, target2.DateTime);
            Assert.Equal(actual.FirstName, target2.FirstName);
            Assert.Equal(actual.LegacyString, target2.LatestString);
            Assert.Null(actual.LegacyTarget1String);
            Assert.Equal(actual.LegacyTarget2String, target2.SourceString);
            Assert.Equal(actual.SealedString, target2.ProtectedString);
            Assert.Null(actual.SealedTarget1String);
            Assert.Equal(actual.SealedTarget2String, target2.TargetString);
        }

        private static Source1 CreateSource1()
        {
            return new Source1
            {
                FirstName = "John",
                DateTime = DateTime.Now,
                LegacyString = "legacy field",
                LegacyTarget1String = "custom field only for 'Target1'",
                LegacyTarget2String = "custom field only for 'Target2'",
                SealedString = "sealed field, we don't change legacy code",
                SealedTarget1String = "custom field only for 'Target1', see [Bind]",
                SealedTarget2String = "custom field only for 'Target2', see [Bind]",
            };
        }

        private static Target1 CreateTarget1()
        {
            return new Target1
            {
                FirstName = "John",
                DateTime = DateTime.Now,
                LatestString = "legacy field",
                SourceString = "custom field only for 'Target1'",
                ProtectedString = "sealed field, we don't change legacy code",
                TargetString = "custom field only for 'Target1', see [Bind]",
            };
        }
        
        private static Target2 CreateTarget2()
        {
            return new Target2
            {
                FirstName = "John",
                DateTime = DateTime.Now,
                LatestString = "legacy field",
                SourceString = "custom field only for 'Target1'",
                ProtectedString = "sealed field, we don't change legacy code",
                TargetString = "custom field only for 'Target1', see [Bind]",
            };
        }

        public class Source1
        {
            [Ignore(typeof(Target1))] // ignore set 'Target1' field 'DateTime', but not 'Target2'
            public DateTime DateTime { get; set; }
            public string FirstName { get; set; }
            
            [Bind("LatestString")] // set to 'Target1' and 'Target2' field 'LatestString'
            public string LegacyString { get; set; }
            [Bind("SourceString", typeof(Target1))] // set to 'Target1' field 'SourceString'
            public string LegacyTarget1String { get; set; }
            [Bind("SourceString", typeof(Target2))] // set to 'Target2' field 'SourceString'
            public string LegacyTarget2String { get; set; }

            public string SealedString { get; set; }
            public string SealedTarget1String { get; set; }
            public string SealedTarget2String { get; set; }
        }

        public class Target1
        {
            public DateTime DateTime { get; set; }
            public string FirstName { get; set; }
            public string LatestString { get; set; }
            public string SourceString { get; set; }

            [Bind("SealedString")] // set from 'Source1' field 'SealedString'
            public string ProtectedString { get; set; }
            [Bind("SealedTarget1String", typeof(Source1))] // set from 'Source1' field 'SealedTarget1String'
            public string TargetString { get; set; }
        }

        public class Target2
        {
            public DateTime DateTime { get; set; }
            public string FirstName { get; set; }
            public string LatestString { get; set; }
            public string SourceString { get; set; }

            [Bind("SealedString")] // set from 'Source1' field 'SealedString'
            public string ProtectedString { get; set; }
            [Bind("SealedTarget2String", typeof(Source1))] // set from 'Source1' field 'SealedTarget2String'
            public string TargetString { get; set; }
        }
    }
}
