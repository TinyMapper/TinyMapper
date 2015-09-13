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
            TinyMapper.Bind<Source, Target>(config => config.Bind(from => @from.Campaign, typeof(Campaign1)));

            var source = new Source
            {
                Campaign = new Campaign1
                {
                    Value = 1
                }
            };

            var result = TinyMapper.Map<Target>(source);
        }

        public class Campaign1 : CampaignBase
        {
        }

        public class Campaign2 : CampaignBase
        {
        }

        public abstract class CampaignBase
        {
            public int Value { get; set; }
        }

        public class Source : SourceBase
        {
        }

        public abstract class SourceBase
        {
            public CampaignBase Campaign { get; set; }
        }

        public class Target : SourceBase
        {
        }
    }
}
