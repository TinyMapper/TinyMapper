using System.ComponentModel;
using BenchmarkDotNet.Attributes;

namespace BenchmarkInternal
{
    public class TypeConverters
    {
        private readonly TypeConverter _intToString = TypeDescriptor.GetConverter(typeof(int));

        [Benchmark]
        public void IntToStringByConverter()
        {
           var result =  _intToString.ConvertTo(1, typeof(string));
        }

        [Benchmark]
        public void IntToStringByToString()
        {
            var result = 1.ToString();
        }
    }
}
