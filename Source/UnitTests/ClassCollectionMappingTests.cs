using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
{
    public sealed class ClassCollectionMappingTests
    {
        [Fact]
        public void Map_ClassCollections_Success()
        {
            TinyMapper.Bind<Class9, Class10>();

            var source = new Class9
            {
                Items = new List<Item1>
                {
                    new Item1
                    {
                        Int = 1,
                        String = "2",
                        List = new List<int> { 1, 2, 3 },
                        Bools = new[] { true, false }
                    },
                    new Item1
                    {
                        Int = 2,
                        String = "3",
                        List = new List<int> { 2, 3 },
                        Bools = new[] { false, false }
                    }
                }
            };

            var actual = TinyMapper.Map<Class10>(source);

            Assert.Equal(source.Items.Count, actual.Items.Count);

            for (int i = 0; i < source.Items.Count; i++)
            {
                Item1 expectedItem = source.Items[i];
                Item2 actualItem = actual.Items[i];

                Assert.Equal(expectedItem.Bools, actualItem.Bools);
                Assert.Equal(expectedItem.Int, actualItem.Int);
                Assert.Equal(expectedItem.List, actualItem.List);
                Assert.Equal(expectedItem.String, actualItem.String);
            }
        }


        public class Class10
        {
            public List<Item2> Items { get; set; }
        }


        public class Class9
        {
            public List<Item1> Items { get; set; }
        }


        public sealed class Item1
        {
            public bool[] Bools { get; set; }
            public int Int { get; set; }
            public List<int> List { get; set; }
            public string String { get; set; }
        }


        public sealed class Item2
        {
            public bool[] Bools { get; set; }
            public int Int { get; set; }
            public List<int> List { get; set; }
            public string String { get; set; }
        }
    }
}
