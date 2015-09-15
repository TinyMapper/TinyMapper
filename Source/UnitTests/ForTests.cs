using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
{
    public sealed class ForTests
    {
        [Fact]
        public void Test()
        {
            TinyMapper.Bind<Source, Target>();

            var source = new Source
            {
                 List = new List<int>{1, 2,3}
            };

            var result = TinyMapper.Map<Target>(source);
        }


        public class Source
        {
            public IReadOnlyList<int> List { get; set; }
        }


        public class Target
        {
            public List<int> List { get; set; }
        }

    }
}
