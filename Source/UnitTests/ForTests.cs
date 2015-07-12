using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Reflection;
using Xunit;

namespace UnitTests
{
    public sealed class ForTests
    {
        [Fact]
        public void Test()
        {
            var source = new Source
            {
                MyInt = 1,
                MyString = "My Value"
            };

            TinyMapper.Bind<Source, Target>(config =>
            {
//                config.Bind(from => from.MyInt, to => to.Int);
//                config.Bind(from => from.MyString, to => to.MyString);
            });

            var target = TinyMapper.Map<Target>(source);
        }

        [Fact]
        public void Test1()
        {
            var source = new Source1
            {
                Dictionary = new Dictionary<string, int> { { "Key", 1 } }
            };

            TinyMapper.Bind<Source1, Target1>();
//            DynamicAssemblyBuilder.Get().Save();
            var target = TinyMapper.Map<Target1>(source);
        }

        public class Source
        {
            public int MyInt { get; set; }
            public string MyString { get; set; }
        }

        public class Source1
        {
            public Dictionary<string, int> Dictionary { get; set; }
        }

        public class Target
        {
            public int Int { get; set; }
            public string MyString { get; set; }
        }

        public class Target1
        {
            public Dictionary<string, int> Dictionary { get; set; }
        }
    }
}
