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
            var fromDatabase = new List<Survey>
            {
                new Survey { Id = 1, Data = "Data1" },
                new Survey { Id = 2, Data = "Data2" }
            };

            TinyMapper.Bind<List<Survey>, List<SurveyDto>>();

            var dto = TinyMapper.Map<List<SurveyDto>>(fromDatabase);
        }


        public sealed class Survey
        {
            public int Id { get; set; }
            public string Data { get; set; }
        }


        public sealed class SurveyDto
        {
            public int Id { get; set; }
            public string Data { get; set; }
        }
    }
}
