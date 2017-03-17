using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests.Mappings
{
    public sealed class MappingWithConfigTests
    {
        [Fact]
        public void Map_Bind_Success()
        {
            TinyMapper.Bind<SourceItem, TargetItem>();

            TinyMapper.Bind<Source1, Target1>(config =>
            {
                config.Bind(from => from.String, to => to.MyString);
                config.Bind(from => from.Int, to => to.MyInt);
                config.Bind(from => from.SourceItem, to => to.TargetItem);
            });

            var source = new Source1
            {
                Bool = true,
                Byte = 5,
                Int = 9,
                String = "Test",
                DateTime = DateTime.Now,
                SourceItem = new SourceItem { Id = Guid.NewGuid() }
            };

            var actual = TinyMapper.Map<Target1>(source);

            Assert.Equal(source.Bool, actual.Bool);
            Assert.Equal(source.String, actual.MyString);
            Assert.Equal(source.Byte, actual.Byte);
            Assert.Equal(source.Int, actual.MyInt);
            Assert.Equal(source.DateTime, actual.DateTime);
            Assert.Equal(source.SourceItem.Id, actual.TargetItem.Id);
        }

        [Fact]
        public void Map_BindCaseSensitive_Success()
        {
            TinyMapper.Bind<SourceItem, TargetItem>();

            TinyMapper.Bind<Source1, Target1>(config =>
            {
                config.Bind(from => from.CaseSensitive, to => to.Casesensitive);
            });

            var source = new Source1
            {
                CaseSensitive = "CaseSensitive"
            };

            var actual = TinyMapper.Map<Target1>(source);

            Assert.Equal(source.CaseSensitive, actual.Casesensitive);
        }

        [Fact]
        public void Map_ConcreteType_Success()
        {
            TinyMapper.Bind<Source2, Target2>(config =>
            {
                config.Bind(target => target.Ints, typeof(List<int>));
                config.Bind(target => target.Strings, typeof(List<string>));
            });

            var source = new Source2
            {
                Ints = new List<int> { 1, 2, 3 },
                Strings = new List<string> { "A", "B", "C" }
            };

            var actual = TinyMapper.Map<Target2>(source);
            Assert.Equal(source.Ints, actual.Ints);
            Assert.Equal(source.Strings, actual.Strings);
        }

        [Fact]
        public void Map_Ignore_Success()
        {
            TinyMapper.Bind<Source1, Target1>(config =>
            {
                config.Ignore(x => x.Bool);
                config.Ignore(x => x.String);
            });

            var source = new Source1
            {
                Bool = true,
                Byte = 5,
                Int = 9,
                String = "Test",
            };

            var actual = TinyMapper.Map<Target1>(source);

            Assert.Equal(false, actual.Bool);
            Assert.Equal(null, actual.MyString);
            Assert.Equal(source.Byte, actual.Byte);
            Assert.Equal(0, actual.MyInt);
            Assert.Null(actual.TargetItem);
        }


        [Fact]
        public void Map_BindIfNotBound_Success()
        {
            TinyMapper.Bind<Source1, Target1>(config =>
            {
                config.Ignore(x => x.Int);
            });
            TinyMapper.BindIfNotBound<Source1, Target1>(config =>
            {
                config.Ignore(x => x.Bool);
            });

            var source = new Source1
            {
                Bool = true,
                Byte = 5,
                Int = 9,
                String = "Test",
            };

            var actual = TinyMapper.Map<Target1>(source);

            Assert.Equal(source.Bool, actual.Bool);
            Assert.Equal(null, actual.MyString);
            Assert.Equal(source.Byte, actual.Byte);
            Assert.Equal(0, actual.MyInt);
            Assert.Null(actual.TargetItem);
        }

        public class Source1
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public int Int { get; set; }
            public DateTime? DateTime { get; set; }
            public string CaseSensitive { get; set; }

            public SourceItem SourceItem { get; set; }
            public string String { get; set; }
        }

        public class Source2
        {
            public IList<int> Ints { get; set; }
            public List<string> Strings { get; set; }
        }

        public class SourceItem
        {
            public Guid Id { get; set; }
        }

        public class Target1
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public int MyInt { get; set; }
            public DateTime? DateTime { get; set; }
            public string MyString { get; set; }
            public string Casesensitive { get; set; }

            public TargetItem TargetItem { get; set; }
        }

        public class Target2
        {
            public IList<int> Ints { get; set; }
            public IEnumerable<string> Strings { get; set; }
        }

        public class TargetItem
        {
            public Guid Id { get; set; }
        }
    }
}
