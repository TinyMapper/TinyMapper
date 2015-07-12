using System;
using Nelibur.ObjectMapper;
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
                config.Bind(from => from.MyInt, to => to.Int);
                config.Bind(from => from.MyString, to => to.MyString);
            });

            var target = TinyMapper.Map<Target>(source);
        }

        public class Source
        {
            public int MyInt { get; set; }
            public string MyString { get; set; }
        }

        public class Target
        {
            public int Int { get; set; }
            public string MyString { get; set; }
        }
    }
}
