using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Reflection;
using Xunit;

namespace UnitTests
{
    public sealed class ForTests
    {
        [Fact(Skip = "ForTests")]
//        [Fact]
        public void Test()
        {
//            TinyMapper.Bind<TagSource, TagSource1>();
            TinyMapper.Bind<SourceTest, TargetTest>(config =>
            {
                config.Bind(from => from.Tag.Id, to => to.Id);
            });

            var source = new SourceTest
            {
                Tag = new TagSource
                {
                    Id = 1
                }
//                 DataSource = "Data"
            };

            DynamicAssemblyBuilder.Get().Save();

            var target = TinyMapper.Map<SourceTest, TargetTest>(source);

        }

//        public TargetTest Test1(SourceTest source, TargetTest target)
//        {
//            target.Id = source.Id;
//            return target;
//        }
//
//        public TargetTest Test2(SourceTest source, TargetTest target)
//        {
//            target.Tag2.Id = source.Id;
//            return target;
//        }

        public TargetTest Test3(SourceTest source, TargetTest target)
        {
            target.Tag.Id = source.Tag.Id;
            return target;
        }

//        public TargetTest Test5(SourceTest source, TargetTest target)
//        {
//            target.Id = source.Tag.Id;
//            return target;
//        }
    }


    public class SourceTest
    {
        public TagSource Tag { get; set; }
    }


    public class TagSource
    {
        public int Id { get; set; }
    }

    public class TagSource1
    {
        public int Id { get; set; }
    }

    public class TargetTest
    {
        public int Id { get; set; }
        public TagSource1 Tag { get; set; }
//        public int Id { get; set; }
    }
}
