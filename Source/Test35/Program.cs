using System;
using Nelibur.ObjectMapper;

namespace Test35
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = new SourceStatic();

            TinyMapper.Bind<SourceStatic, TargetDto>();
            var actual = TinyMapper.Map<TargetDto>(source);
        }
    }


    public class SourceStatic
    {
        public static string Name = "test";
        public static int Id = 1;
    }


    public class TargetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
