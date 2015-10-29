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
            //                        TinyMapper.Bind<Source, Target>(config => config.Bind(target => target.Value, "MyValue"));
            //
            //            var source = new Source();
            //            var result = TinyMapper.Map<Target>(source);
        }

        [Fact(Skip = "For test")]
        public void Test_Area()
        {
            TinyMapper.Bind<SourceArea, TargetArea>(config => config.Bind(source => source.Area.Name, target => target.AreaName));

            var actual = new SourceArea
            {
                Area = new Area { Name = "MyName" }
            };

            var result = TinyMapper.Map<TargetArea>(actual);
        }


        public class Area
        {
            public string Name { get; set; }
        }


        public class Source
        {
            public string Value { get; set; }
        }


        public class SourceArea
        {
            public Area Area { get; set; }
        }


        public class Target
        {
            public string Value { get; set; }
        }


        public class TargetArea
        {
            public string AreaName { get; set; }
        }
    }
}
