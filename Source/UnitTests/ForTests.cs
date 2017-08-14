using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Reflection;
using Xunit;

namespace UnitTests
{
    public sealed class ForTests
    {
//        [Fact(Skip = "ForTests")]
        [Fact]
        public void Test()
        {
            TinyMapper.Bind<SourceTest, TargetTest>();

            var source = new SourceTest
            {
                List = new List<int>{1, 2, 3}
            };

            DynamicAssemblyBuilder.Get().Save();

            var target = TinyMapper.Map<SourceTest, TargetTest>(source);

            Assert.Equal(source.List, target.List);
        }
    }


    public class SourceTest
    {
        public List<int> List { get; set; }
    }

    public class TargetTest
    {
        public List<int> List { get; set; }
    }
}
