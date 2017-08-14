using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Nelibur.ObjectMapper;

namespace BenchmarkInternal
{
    public class Benchmark
    {
        private const int Iterations = 10;

        private readonly SourceTest _sourceTest = CreateSource();

        public Benchmark()
        {
            TinyMapper.Bind<SourceTest, TargetTest>();
        }

        [Benchmark]
        public void TinyMapperCollectionMapping()
        {
            for (int i = 0; i < Iterations; i++)
            {
                TinyMapper.Map<SourceTest, TargetTest>(_sourceTest);
            }
        }

        [Benchmark]
        public void HandwrittenCollectionMapping()
        {
            for (int i = 0; i < Iterations; i++)
            {
                var result = new TargetTest
                {
                    List = new List<int>()
                };
                result.List.AddRange(_sourceTest.List);
            }
        }

        private static SourceTest CreateSource()
        {
            var result = new SourceTest
            {
                List = new List<int>()
            };

            for (int i = 0; i < 100; i++)
            {
                result.List.Add(i);
            }
            return result;
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
