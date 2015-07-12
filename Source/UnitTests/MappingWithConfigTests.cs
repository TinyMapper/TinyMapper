using System;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
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
                SourceItem = new SourceItem { Id = Guid.NewGuid() }
            };

            var actual = TinyMapper.Map<Target1>(source);

            Assert.Equal(source.Bool, actual.Bool);
            Assert.Equal(source.String, actual.MyString);
            Assert.Equal(source.Byte, actual.Byte);
            Assert.Equal(source.Int, actual.MyInt);
            Assert.Equal(source.SourceItem.Id, actual.TargetItem.Id);
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

        public class Source1
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public int Int { get; set; }

            public SourceItem SourceItem { get; set; }
            public string String { get; set; }
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
            public string MyString { get; set; }

            public TargetItem TargetItem { get; set; }
        }

        public class TargetItem
        {
            public Guid Id { get; set; }
        }
    }
}
