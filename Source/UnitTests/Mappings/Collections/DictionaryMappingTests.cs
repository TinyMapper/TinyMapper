using System;
using System.Collections.Generic;
using System.Linq;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests.Mappings.Collections
{
    public sealed class DictionaryMappingTests
    {
        [Fact]
        public void Map_Dictionary_Success()
        {
            TinyMapper.Bind<Source1, Target1>();

            var source = new Source1
            {
                Id = Guid.NewGuid(),
                Dictionary = new Dictionary<string, int> { { "Key1", 1 }, { "Key2", 2 } }
            };

            var target = TinyMapper.Map<Target1>(source);

            Assert.Equal(source.Id, target.Id);
            Assert.Equal(source.Dictionary, target.Dictionary);
        }

        [Fact]
        public void Map_IDictionary_Success()
        {
            TinyMapper.Bind<Source3, Target3>();

            var source = new Source3
            {
                Id = Guid.NewGuid(),
                Dictionary = new Dictionary<string, int> { { "Key1", 1 }, { "Key2", 2 } }
            };

            var target = TinyMapper.Map<Target3>(source);

            Assert.Equal(source.Id, target.Id);
            Assert.Equal(source.Dictionary, target.Dictionary);
        }

        [Fact]
        public void Map_IDictionary_Target_Success()
        {
            TinyMapper.Bind<Source1, Target3>();

            var source = new Source1
            {
                Id = Guid.NewGuid(),
                Dictionary = new Dictionary<string, int> { { "Key1", 1 }, { "Key2", 2 } }
            };

            var target = TinyMapper.Map<Target3>(source);

            Assert.Equal(source.Id, target.Id);
            Assert.Equal(source.Dictionary, target.Dictionary);
        }

        [Fact]
        public void Map_DifferentKeyDictionary_Success()
        {
            TinyMapper.Bind<ItemKeySource, ItemKeyTarget>();
            TinyMapper.Bind<Source2, Target2>();

            var source = new Source2
            {
                Id = Guid.NewGuid(),
                Dictionary = new Dictionary<ItemKeySource, int>
                {
                    { new ItemKeySource { Id = Guid.NewGuid() }, 1 },
                    { new ItemKeySource { Id = Guid.NewGuid() }, 2 },
                }
            };

            var target = TinyMapper.Map<Target2>(source);

            Assert.Equal(source.Id, target.Id);
            Assert.Equal(source.Dictionary.Keys.Select(x => x.Id), target.Dictionary.Keys.Select(x => x.Id));
            Assert.Equal(source.Dictionary.Values, target.Dictionary.Values);
        }

        public class ItemKeySource
        {
            public Guid Id { get; set; }
        }

        public class ItemKeyTarget
        {
            public Guid Id { get; set; }
        }

        public class Source1
        {
            public Dictionary<string, int> Dictionary { get; set; }
            public Guid Id { get; set; }
        }

        public class Source2
        {
            public Dictionary<ItemKeySource, int> Dictionary { get; set; }
            public Guid Id { get; set; }
        }

        public class Source3
        {
            public IDictionary<string, int> Dictionary { get; set; }
            public Guid Id { get; set; }
        }

        public class Target1
        {
            public Dictionary<string, int> Dictionary { get; set; }
            public Guid Id { get; set; }
        }

        public class Target2
        {
            public Dictionary<ItemKeyTarget, int> Dictionary { get; set; }
            public Guid Id { get; set; }
        }

        public class Target3
        {
            public IDictionary<string, int> Dictionary { get; set; }
            public Guid Id { get; set; }
        }
    }
}
