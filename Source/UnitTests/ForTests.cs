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
            //                        TinyMapper.Bind<Source, Target>(config => config.Bind(target => target.Value, "MyValue"));
            //
            //            var source = new Source();
            //            var result = TinyMapper.Map<Target>(source);

            TinyMapper.Bind<PersonComplexFrom, PersonTo>(config =>
            {
                config.Bind(from => from.Nickname, to => to.StringTarget);
            });

//            TinyMapper.Bind<PersonComplexFrom, PersonTo>();

            var person = new PersonComplexFrom
            {
                CreateTime = DateTime.Now,
                Nickname = "Object Mapper",
                Int = 3
            };

//            List<PersonComplexFrom> list = new List<PersonComplexFrom> { person };

            //person.Address = null;
            var personDto = TinyMapper.Map<PersonTo>(person);

//            var lists = TinyMapper.Map<List<PersonTo>>(list);
        }

        public sealed class PersonComplexFrom
        {
            public DateTime? CreateTime { get; set; }

            public int? Int { get; set; }

            public string Nickname { get; set; }
        }

        public sealed class PersonTo
        {
            public DateTime? CreateTime { get; set; }
            public string StringTarget { get; set; }

            public int? Int { get; set; }

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
