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

            TinyMapper.Bind<TestParent, TestDto>(cfg =>
            {
                cfg.Bind(e => e.Sub.Text, e => e.Text);
            });

            var obj = new TestParent { Sub = new TestInherited { Text = "Test" } };

            DynamicAssemblyBuilder.Get().Save();

            var dto = TinyMapper.Map<TestParent, TestDto>(obj);
        }
    }


    public class TestBase
    {
        public virtual string Text { get; set; }
    }

    public class TestInherited : TestBase
    {
        public override string Text { get; set; }
    }

    public class TestParent
    {
        public virtual TestBase Sub { get; set; }
    }

    public class TestDto
    {
        public string Text { get; set; }
    }

}
